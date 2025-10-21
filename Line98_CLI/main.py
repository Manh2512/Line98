import os
from utils import *

def clear_terminal():
    os.system('cls' if os.name == 'nt' else 'clear')

board = Board()
board.initialize()
score = Score()

while True:
    clear_terminal()
    # Generate and visualize prediction
    board.visualize()
    score.visualize()
    
    # Player's movement
    x_begin, y_begin = map(int, input("Move from: ").split())
    if not board.isBall(x_begin, y_begin):
        continue
    x_end, y_end = map(int, input("Move to: ").split())
    valid, added_score = board.move(x_begin, y_begin, x_end, y_end)
    if not valid:
        continue
    score.score += added_score
    if added_score > 0:
        # If player scores, then not generate new balls
        continue
    clear_terminal()
    # Show prediction after movement and scoring with new balls if any
    board.show_next()
    score.score += board.proceed_next()
    board.generate_next()
    
    if board.game_end():
        break