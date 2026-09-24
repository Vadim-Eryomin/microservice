class ICommand:
    def execute(self):
        pass


class MoveCommand(ICommand):
    def __init__(self, obj):
        self.obj = obj

    def execute(self):
        self.obj.journal.pos = self.obj.journal.pos + self.obj.journal.vel


class MacroCommand(ICommand):
    def __init__(self, cmds: list[ICommand]):
        self.cmds = cmds

    def execute(self):
        for cmd in self.cmds:
            cmd.execute()


class StartTransactionCommand(ICommand):
    def __init__(self,obj):
        self.obj = obj

    def execute(self):
        self.obj.journal = self.obj.copy()


class EndTransactionCommand(ICommand):
    def __init__(self, obj):
        self.obj = obj

    def execute(self):
        self.obj = self.obj.journal


class TransactionJournal:
    def __init__(self):
        self.journal = {}

class TransactionManager:
    def __init__(self):
        self.trans = {}