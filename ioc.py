class _IoC:
    def __init__(self):
        self.deps = {'register': lambda args: register(self.deps, args[0], args[1])}

    def resolve(self, key, *args):
        if key in self.deps:
            return self.deps[key](*args)

        return None


def register(deps, key, what):
    deps[key] = what


ioc = _IoC()
