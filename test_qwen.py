from abc import abstractmethod
from dataclasses import dataclass, field
from enum import Enum
from typing import Protocol, Any, Dict


class ICommand(Protocol):
    @abstractmethod
    def execute(self) -> None: ...


class IMovable(Protocol):
    position: tuple[int, int]
    velocity: tuple[int, int]


class ITransactionLog(Protocol):
    records: Dict[str, "TransactionRecord"]


class TxStatus(Enum):
    PENDING = "pending"
    COMMITTED = "committed"
    ROLLED_BACK = "rolled_back"


@dataclass
class TransactionRecord:
    tx_id: str
    status: TxStatus
    command: ICommand


@dataclass
class TransactionLog:
    records: Dict[str, TransactionRecord] = field(default_factory=dict)


@dataclass
class MovableData:
    position: tuple[int, int]
    velocity: tuple[int, int]


class MoveCommand:
    def __init__(self, obj: IMovable):
        self._obj = obj

    def execute(self) -> None:
        x, y = self._obj.position
        dx, dy = self._obj.velocity
        self._obj.position = (x + dx, y + dy)


class PrepareCommand:
    def __init__(self, tx_id: str, command: ICommand, log: ITransactionLog):
        self._tx_id = tx_id
        self._command = command
        self._log = log

    def execute(self) -> None:
        self._log.records[self._tx_id] = TransactionRecord(
            tx_id=self._tx_id,
            status=TxStatus.PENDING,
            command=self._command
        )


class CommitCommand:
    def __init__(self, tx_id: str, log: ITransactionLog):
        self._tx_id = tx_id
        self._log = log

    def execute(self) -> None:
        record = self._log.records.get(self._tx_id)
        if record and record.status == TxStatus.PENDING:
            record.command.execute()
            record.status = TxStatus.COMMITTED


class RollbackCommand:
    def __init__(self, tx_id: str, log: ITransactionLog):
        self._tx_id = tx_id
        self._log = log

    def execute(self) -> None:
        record = self._log.records.get(self._tx_id)
        if record and record.status == TxStatus.PENDING:
            record.status = TxStatus.ROLLED_BACK


class IoCContainer:
    def __init__(self):
        self._registry: Dict[str, Any] = {}

    def register(self, key: str, instance: Any) -> None:
        self._registry[key] = instance

    def resolve(self, key: str) -> Any:
        return self._registry[key]


if __name__ == "__main__":
    container = IoCContainer()

    ship = MovableData(position=(0, 0), velocity=(10, 5))
    log = TransactionLog()

    container.register("IMovable", ship)
    container.register("ITransactionLog", log)

    move = MoveCommand(container.resolve("IMovable"))

    PrepareCommand("tx-001", move, container.resolve("ITransactionLog")).execute()
    CommitCommand("tx-001", container.resolve("ITransactionLog")).execute()

    print(ship.position)
    print(log.records["tx-001"].status)
    