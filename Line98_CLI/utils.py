from random import randint

# Global variables

class Score:
    def __init__(self):
        self.score = 0
    def visualize(self):
        print("Your score is:", self.score)

class Board:
    def __init__(self, begin=7, predict=3):
        self.board = [
            [0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0]
        ]
        self.colors = ["none", "red", "pink", "brown", "yellow", "green", "pale blue", "dark blue"] # 7 colors, 1 blank
        self.begin = begin
        self.predict = predict
        self.next_balls = []

# Define functions
    def initialize(self):
        for i in range(self.begin):
            random_color = randint(1, 7) # Randomly choose color to initialize
            x = randint(0, 8)
            y = randint(0, 8)
            while self.board[x][y] != 0:
                x = randint(0, 8)
                y = randint(0, 8)
            self.board[x][y] = random_color
        self.generate_next()
            
    def visualize(self):
        # Visualize the board
        print("   0  1  2  3  4  5  6  7  8")
        color_map = {
            0: "37",                    # blank
            1: "31",                    # red
            2: "35",                    # pink / magenta
            3: "38;5;96",               # brown
            4: "33",                    # yellow
            5: "32",                    # green
            6: "38;2;135;206;235",      # pale blue
            7: "34"                     # dark blue
        }
        # print("\033[31mThis is red text\033[0m")
        count = 0
        for i in range(9):
            print(i, end="  ")
            for j in range(9):
                pred = False
                if count < 3:
                    for ball in self.next_balls:
                        if ball[1] == i and ball[2] == j:
                            count += 1
                            print(f"\033[{color_map[ball[0]]}m0\033[0m", end="  ")
                            pred = True
                            break
                if not pred:
                    print(f"\033[{color_map[self.board[i][j]]}m{self.board[i][j]}\033[0m", end="  ")
            print("", end='\n')
            
    def generate_next(self, predict=3):
        self.next_balls = []
        for i in range(predict):
            random_color = randint(1, 7) # Randomly choose color to initialize
            x = randint(0, 8)
            y = randint(0, 8)
            while self.board[x][y] != 0:
                x = randint(0, 8)
                y = randint(0, 8)
            self.next_balls.append((random_color, x, y))
            
        self.visualize_predict()
        return self.next_balls
    
    def visualize_predict(self):
        for (color, x, y) in self.next_balls:
            print("Next places are:")
            print(f"{color}, at ({x}, {y})")
    
    def show_next(self):
        # display predicted balls as big balls after movement
        for i in range(self.predict):
            color, x, y = self.next_balls[i]
            while self.board[x][y] != 0:
                # randomly move to other empty place
                x = randint(0, 8)
                y = randint(0, 8)
            self.next_balls[i] = (color, x, y)
            self.board[x][y] = color
        self.visualize()
            
    def proceed_next(self):
        # Handling scoring that may happen after showing the predict balls
        score = 0
        for (_, x, y) in self.next_balls:
            score += self.scoring(x, y)
        return score

    def isBall(self, x_begin, y_begin):
        if (self.board[x_begin][y_begin] == 0):
            print("No ball is here!")
            return False
        return True
    
    def findVertical(self, x0, y0):
        vertical = [(x0, y0)]
        n = 0
        visited = [False, False, False, False, False, False, False, False, False]
        visited[x0] = True
        
        while n < len(vertical):
            x, y = vertical[n]
            if x > 0 and self.board[x-1][y] == self.board[x0][y0] and not visited[x-1]:
                vertical.append((x-1, y))
                visited[x-1] = True
            if x < 8 and self.board[x+1][y] == self.board[x0][y0] and not visited[x+1]:
                vertical.append((x+1, y))
                visited[x+1] = True
            n += 1
        
        return vertical
    
    def findHorizontal(self, x0, y0):
        horizontal = [(x0, y0)]
        n = 0
        visited = [False, False, False, False, False, False, False, False, False]
        visited[y0] = True
        
        while n < len(horizontal):
            x, y = horizontal[n]
            if y > 0 and self.board[x][y-1] == self.board[x0][y0] and not visited[y-1]:
                horizontal.append((x, y-1))
                visited[y-1] = True
            if y < 8 and self.board[x][y+1] == self.board[x0][y0] and not visited[y+1]:
                horizontal.append((x, y+1))
                visited[y+1] = True
            n += 1
        
        return horizontal
    
    def findLeftRight(self, x0, y0):
        sequence = [(x0, y0)]
        n = 0
        visited = [False, False, False, False, False, False, False, False, False]
        visited[x0] = True
        
        while n < len(sequence):
            x, y = sequence[n]
            if x > 0 and y > 0 and self.board[x-1][y-1] == self.board[x0][y0] and not visited[x-1]:
                sequence.append((x-1, y-1))
                visited[x-1] = True
            if x < 8 and y < 8 and self.board[x+1][y+1] == self.board[x0][y0] and not visited[x+1]:
                sequence.append((x+1, y+1))
                visited[x+1] = True
            n += 1
        
        return sequence

    def findRightLeft(self, x0, y0):
        sequence = [(x0, y0)]
        n = 0
        visited = [False, False, False, False, False, False, False, False, False]
        visited[x0] = True
        
        while n < len(sequence):
            x, y = sequence[n]
            if x > 0 and y < 8 and self.board[x-1][y+1] == self.board[x0][y0] and not visited[x-1]:
                sequence.append((x-1, y+1))
                visited[x-1] = True
            if x < 8 and y > 0 and self.board[x+1][y-1] == self.board[x0][y0] and not visited[x+1]:
                sequence.append((x+1, y-1))
                visited[x+1] = True
            n += 1
        
        return sequence
    
    def foundSequence(self, sequence):
        score = 0
        if len(sequence) >=5:
            score += len(sequence)
            for (x, y) in sequence:
                self.board[x][y] = 0
        return score
    
    def scoring(self, x0, y0):
        # Check around x, y to see if there is sequence of >5 balls
        # Use recurrent
        score = 0
        vertical = self.findVertical(x0, y0)
        horizontal = self.findHorizontal(x0, y0)
        leftright = self.findLeftRight(x0, y0)
        rightleft = self.findRightLeft(x0, y0)
        
        score += self.foundSequence(vertical)
        score += self.foundSequence(horizontal)
        score += self.foundSequence(leftright)
        score += self.foundSequence(rightleft)
        return score
        
    
    def move(self, x_begin, y_begin, x_end, y_end):
        # Move a chosen ball at (x_begin, y_begin) to (x_end, y_end)
        # x's and y's in 0-based indexing
        # Check if the move is valid by BFS
        if (self.board[x_end][y_end] != 0):
            # Replace to new begin position later
            print("Invalid move!")
            return False, 0
        adjacent = [(x_begin, y_begin)]
        valid = False
        visited = [
            [False, False, False, False, False, False, False, False, False],
            [False, False, False, False, False, False, False, False, False],
            [False, False, False, False, False, False, False, False, False],
            [False, False, False, False, False, False, False, False, False],
            [False, False, False, False, False, False, False, False, False],
            [False, False, False, False, False, False, False, False, False],
            [False, False, False, False, False, False, False, False, False],
            [False, False, False, False, False, False, False, False, False],
            [False, False, False, False, False, False, False, False, False]
        ]
        visited[x_begin][y_begin] = True
        while len(adjacent) != 0:
            x, y = adjacent.pop()
            if x == x_end and y == y_end:
                valid = True
                break
            if x > 0 and self.board[x-1][y] == 0 and not visited[x-1][y]: # Up
                adjacent.append((x-1, y))
                visited[x-1][y] = True
            if y > 0 and self.board[x][y-1] == 0 and not visited[x][y-1]: # Left
                adjacent.append((x, y-1))
                visited[x][y-1] = True
            if x < 8 and self.board[x+1][y] == 0 and not visited[x+1][y]: # Down
                adjacent.append((x+1, y))
                visited[x+1][y] = True
            if y < 8 and self.board[x][y+1] == 0 and not visited[x][y+1]: # Right
                adjacent.append((x, y+1))
                visited[x][y+1] = True
        if valid:
            self.board[x_end][y_end] = self.board[x_begin][y_begin]
            self.board[x_begin][y_begin] = 0
            # Check if the move scores
            score = self.scoring(x_end, y_end)
            return True, score
        else:
            print("Invalid move!")
            return False, 0
        

    def game_end(self):
        # Game ended when there is 3 empty space after scoring with prediction
        empty = 0
        for i in range(9):
            for j in range(9):
                if self.board[i][j] == 0:
                    empty += 1
        if empty <= 3:
            return True
        return False