from Command import MacroCommand, ICommand


class IoC:
    def __init__(self):
        self.deps = {'register': lambda args: register(self.deps, args[0], args[1])}
        # self.deps = {'register': lambda args: print(args)}

    def resolve(self, key, *args):
        if key in self.deps:
            return self.deps[key](args)

        return None


def register(deps, key, what):
    deps[key] = what


ioc = IoC()
ioc.resolve("register", "macro", lambda args: MacroCommand(args))


class PrintCommand(ICommand):
    def execute(self):
        print("Hello!")

command = ioc.resolve("macro", PrintCommand())
command.execute()
