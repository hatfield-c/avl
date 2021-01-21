
def printHeading1(msg = "", delimCount = 120):
    leftOffset = int((delimCount / 2) - (len(msg) / 2))

    print("\n")
    printDelim(delim = "=", amount = delimCount)
    print("")
    printLine(indentNum = leftOffset, msg = msg, indent = " ")
    print("")
    printDelim(delim = "=", amount = delimCount)
    print("")

def printHeading2(msg = "", delimCount = 120):
    leftOffset = int((delimCount / 2) - (len(msg) / 2))

    print("\n")
    printDelim(delim = "*", amount = delimCount)
    printLine(indentNum = leftOffset, msg = msg, indent = " ")
    printDelim(delim = "*", amount = delimCount)
    print("")

def printHeading3(msg = "", delimCount = 120):
    leftOffset = int((delimCount / 2) - (len(msg) / 2))

    print("\n")
    printDelim(delim = "-", amount = delimCount)
    printLine(indentNum = leftOffset, msg = msg, indent = " ")
    printDelim(delim = "-", amount = delimCount)
    print("")

def printHeading4(msg = "", delimCount = 120):
    leftOffset = int((delimCount / 2) - (len(msg) / 2))

    print("\n")
    printLine(indentNum = leftOffset, msg = msg, indent = " ")
    printDelim(delim = "-", amount = delimCount)
    print("")

def printDelim(delim = "-", amount = 1):
    d = getRepeatStr(s = delim, n = amount)

    print(d)

def printLine(indentNum = 0, msg = "", indent = "    "):
    indentations = getRepeatStr(s = indent, n = indentNum)

    print(indentations + str(msg))

def getRepeatStr(s = "", n = 1):
    r = ""

    for i in range(n):
        r = r + str(s)

    return r