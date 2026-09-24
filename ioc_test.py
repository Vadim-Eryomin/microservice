from abc import ABC, abstractmethod
from typing import Dict, Any, List, Tuple

# --- 1. Модели ---

class Ship(ABC):
    """Абстрактный класс для корабля."""
    def __init__(self, ship_id: str, location: Tuple[int, int]):
        self.ship_id = ship_id
        self.location = location

    @abstractmethod
    def move(self, new_location: Tuple[int, int]) -> bool:
        """Метод для перемещения корабля (атомарно)."""
        pass

    def __repr__(self):
        return f"Ship(ID='{self.ship_id}', Location=({self.location[0]}, {self.location[1]}))"

# --- 2. Сервисы (Интерфейсы и Конкретные Реализации) ---

class ShipService:
    """Сервис для управления состоянием кораблей (Business Logic)."""
    def __init__(self):
        # Имитация базы данных
        self._ships: Dict[str, Ship] = {}

    def add_ship(self, ship: Ship):
        self._ships[ship.ship_id] = ship

    def get_ship(self, ship_id: str) -> Ship | None:
        return self._ships.get(ship_id)

    def update_ship_location(self, ship_id: str, new_location: Tuple[int, int]) -> bool:
        """Атомарная операция перемещения."""
        ship = self.get_ship(ship_id)
        if not ship:
            return False
        
        ship.location = new_location
        print(f"[ShipService] SUCCESS: Ship {ship_id} moved to {new_location}")
        return True

# --- 3. Паттерн Команда (Command Pattern) ---

class Command(ABC):
    """Абстрактный класс для команды."""
    @abstractmethod
    def getData(self) -> Dict[str, Any]:
        """Возвращает данные, необходимые для команды."""
        pass

class MoveShipCommand(Command):
    """Конкретная команда: Перемещение корабля. Инкапсулирует данные."""
    def __init__(self, ship_id: str, new_location: Tuple[int, int]):
        self.ship_id = ship_id
        self.new_location = new_location

    def getData(self) -> Dict[str, Any]:
        """Возвращает данные транзакции."""
        tx_id = f"{self.ship_id}-{hash(self.new_location)}"
        return {
            "tx_id": tx_id,
            "ship_id": self.ship_id,
            "new_location": self.new_location
        }

# --- 4. Менеджеры и IoC (Dependency Injection) ---

class TwoPhaseCommitManager:
    """Менеджер, реализующий 2PC."""
    def __init__(self, ship_service: ShipService):
        self.ship_service = ship_service
        self._pending_transactions: Dict[str, Dict[str, Any]] = {}

    def prepare(self, transaction_data: Dict[str, Any]) -> bool:
        print(f"[2PC Manager] Preparing transaction: {transaction_data.get('tx_id')}")
        if transaction_data.get('ship_id') in self.ship_service._ships:
            self._pending_transactions[transaction_data.get('tx_id')] = transaction_data
            return True
        return False

    def commit(self, transaction_data: Dict[str, Any]) -> bool:
        tx_id = transaction_data.get('tx_id')
        if tx_id not in self._pending_transactions:
            print(f"[2PC Manager] Error: Transaction {tx_id} not found for commit.")
            return False
        
        ship_id = transaction_data.get('ship_id')
        new_loc = transaction_data.get('new_location')
        success = self.ship_service.update_ship_location(ship_id, new_loc)
        
        if success:
            del self._pending_transactions[tx_id]
            print(f"[2PC Manager] Transaction {tx_id} successfully COMMITTED.")
            return True
        else:
            print(f"[2PC Manager] ERROR: Failed to commit due to service failure.")
            return False

    def rollback(self, transaction_data: Dict[str, Any]) -> bool:
        tx_id = transaction_data.get('tx_id')
        if tx_id in self._pending_transactions:
            del self._pending_transactions[tx_id]
            print(f"[2PC Manager] Transaction {tx_id} successfully ROLLED BACK.")
            return True
        return True


# --- 5. Контейнер и Сервер (IoC Container) ---

class IoCContainer:
    """Контейнер для регистрации и предоставления зависимостей."""
    def __init__(self):
        self._services: Dict[str, Any] = {}

    def register(self, name: str, instance: Any):
        self._services[name] = instance

    def resolve(self, name: str) -> Any:
        if name not in self._services:
            raise ValueError(f"Service '{name}' not registered.")
        return self._services[name]

class ShipServer:
    """Сервер, который использует внедренные зависимости."""
    def __init__(self, container: IoCContainer):
        # Получаем зависимости из контейнера (IoC)
        self.ship_service = container.resolve("ShipService")
        
        # Инициализация 2PC, зависящей от сервиса
        self.tx_manager = TwoPhaseCommitManager(self.ship_service)

        # Инициализация данных при старте: Ship(ID, (X, Y))
        self.ship_service.add_ship(Ship("S001", (0, 0)))
        print("ShipServer initialized via IoC. Ready to process commands.")

    def get_ship_status(self, ship_id: str) -> Tuple[int, int] | str:
        ship = self.ship_service.get_ship(ship_id)
        if ship:
            return ship.location
        return "Ship not found"


# --- Инициализация и Тест (Demo) ---

if __name__ == "__main__":
    # 1. Настройка IoC-Контейнера (Регистрация зависимостей)
    container = IoCContainer()
    
    # Регистрация сервисов
    container.register("ShipService", ShipService())

    # 2. Инициализация Сервера (внедрение зависимостей)
    server = ShipServer(container)
    
    # 3. Инициализация Клиента (Клиент теперь работает с командой)
    
    # Клиент получает доступ к менеджеру транзакций
    client_tx_manager = server.tx_manager 

    print("\n" + "="*60)
    print("START SEQUENCE: Ship S001 is at", server.get_ship_status("S001"))
    print("="*60)

    # Клиент создает команду (инкапсулирует данные)
    move_command = MoveShipCommand(
        ship_id="S001", 
        new_location=(10, 5)  # Новые координаты (X=10, Y=5)
    )

    # Клиент управляет выполнением команды (Оркестрация 2PC)
    transaction_data = move_command.getData()
    
    if not client_tx_manager.prepare(transaction_data):
        print("Process aborted: Preparation failed.")
    else:
        if client_tx_manager.commit(transaction_data):
            print("Process successfully committed.")
        else:
            client_tx_manager.rollback(transaction_data)
            print("Process failed and rolled back.")
            
    print("\n" + "="*60)
    if server.get_ship_status("S001") != "Ship not found":
        print("FINAL STATUS: Ship S001 is now at", server.get_ship_status("S001"))
    else:
        print("FINAL STATUS: Operation failed.")
    print("="*60)
