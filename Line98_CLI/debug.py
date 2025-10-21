from app import *

# Testing functions
board = Board()
board.initialize()

def testVertical(x0, y0):
    for i in range(0, 6):
        board.board[x0+i][y0] = 4
    board.visualize()
    sequence = board.findVertical(x0, y0)
    return sequence

def testHorizontal(x0, y0):
    for i in range(0, 6):
        board.board[x0][y0+i-1] = 4
    board.visualize()
    sequence = board.findHorizontal(x0, y0)
    return sequence

def testLeftRight(x0, y0):
    for i in range(0, 6):
        board.board[x0+i][y0+i] = 4
    board.visualize()
    sequence = board.findLeftRight(x0, y0)
    return sequence

def testRightLeft(x0, y0):
    for i in range(0, 6):
        board.board[x0-i][y0+i] = 4
    board.visualize()
    sequence = board.findRightLeft(x0, y0)
    print(testScoring(x0, y0, 0))
    return sequence

def testScoring(x0, y0, score):
    board.scoring(x0, y0, score)
    return score

print(testHorizontal(6, 1))