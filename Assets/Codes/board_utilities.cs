using UnityEngine;
using System;
using System.Collections.Generic;

namespace BoardUtilities{
    public static class Board {
        public static int[,] ball_map = {
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0}
        };
        private static string[] colors = { "none", "red", "pink", "brown", "yellow", "green", "pale blue", "dark blue" };
        private static int begin = 7;
        private static int predict = 3;
        public static int[,] next_balls = new int[3, 3];

        public static void initialize(){
            for(int i=0; i<begin; i++){
                System.Random random = new System.Random();
                int random_color = random.Next(1, 8);
                int x = random.Next(0, 9);
                int y = random.Next(0, 9);
                while(ball_map[x,y] != 0){
                    x = random.Next(0, 9);
                    y = random.Next(0, 9);
                }
                ball_map[x, y] = random_color;
            }
        }

        // Omit: visualize function for board on terminal
        private static bool duplicate_remove(int k, int x, int y){
            // Avoid duplication when generate next balls
            for(int i=0; i<next_balls.GetLength(0); i++){
                if(k == i){
                    continue;
                }
                if(x == next_balls[i,1] && y == next_balls[i,2]){
                    return true;
                }
            }
            return false;
        }
        public static void generate_next(){
            System.Random random = new System.Random();
            // Generate prediction for next ball
            next_balls = new int[3, 3];
            for(int i=0; i<predict; i++){
                int random_color = random.Next(1, 8);
                int x = random.Next(0, 9);
                int y = random.Next(0, 9);
                while(ball_map[x, y] != 0 || duplicate_remove(i, x, y)){
                    x = random.Next(0, 9);
                    y = random.Next(0, 9);
                }
                next_balls[i, 0] = random_color;
                next_balls[i, 1] = x;
                next_balls[i, 2] = y;
            }
            // Omit: visualize the prediction
        }

        // Omit: visualize the prediction
        public static void show_next(){
            System.Random random = new System.Random();
            // display predicted balls as real balls after movement
            for(int i=0; i<predict; i++){
                int color = next_balls[i, 0];
                int x = next_balls[i, 1];
                int y = next_balls[i, 2];
                while(ball_map[x,y] != 0 || duplicate_remove(i, x, y)){
                    x = random.Next(0, 9);
                    y = random.Next(0, 9);
                }
                next_balls[i, 1] = x;
                next_balls[i, 2] = y;
                ball_map[x,y] = color;
            }
            // Omit visualize
        }

        static bool isBall(int x_begin, int y_begin){
            if(ball_map[x_begin, y_begin] == 0){
                Debug.Log("No ball is here!");
                return false;
            }
            return true;
        }
        static List<int[]> findVertical(int x0, int y0){
            List<int[]> vertical = new List<int[]>{
                new int[] {x0, y0}
            };
            int n = 0;
            bool[] visited = new bool[9] {false, false, false, false, false, false, false, false, false};
            visited[x0] = true;

            while(n < vertical.Count){
                int x = vertical[n][0];
                int y = vertical[n][1];
                if(x > 0 && ball_map[x-1,y]==ball_map[x0,y0] && !visited[x-1]){
                    vertical.Add(new int[]{x-1,y});
                    visited[x-1] = true;
                }
                if(x < 8 && ball_map[x+1,y]==ball_map[x0,y0] && !visited[x+1]){
                    vertical.Add(new int[]{x+1, y});
                    visited[x+1] = true;
                }
                n++;
            }

            return vertical;
        }
        static List<int[]> findHorizontal(int x0, int y0){
            List<int[]> horizontal = new List<int[]>{
                new int[] {x0, y0}
            };
            int n = 0;
            bool[] visited = new bool[9] {false, false, false, false, false, false, false, false, false};
            visited[y0] = true;

            while(n < horizontal.Count){
                int x = horizontal[n][0];
                int y = horizontal[n][1];
                if(y > 0 && ball_map[x,y-1]==ball_map[x0,y0] && !visited[y-1]){
                    horizontal.Add(new int[]{x,y-1});
                    visited[y-1] = true;
                }
                if(y < 8 && ball_map[x,y+1]==ball_map[x0,y0] && !visited[y+1]){
                    horizontal.Add(new int[]{x, y+1});
                    visited[y+1] = true;
                }
                n++;
            }

            return horizontal;
        }

        static List<int[]> findLeftRight(int x0, int y0){
            List<int[]> sequence = new List<int[]>{
                new int[] {x0, y0}
            };
            int n = 0;
            bool[] visited = new bool[9] {false, false, false, false, false, false, false, false, false};
            visited[x0] = true;

            while(n < sequence.Count){
                int x = sequence[n][0];
                int y = sequence[n][1];
                if(x > 0 && y > 0 && ball_map[x-1,y-1]==ball_map[x0,y0] && !visited[x-1]){
                    sequence.Add(new int[]{x-1,y-1});
                    visited[x-1] = true;
                }
                if(x < 8 && y < 8 && ball_map[x+1,y+1]==ball_map[x0,y0] && !visited[x+1]){
                    sequence.Add(new int[]{x+1, y+1});
                    visited[x+1] = true;
                }
                n++;
            }

            return sequence;
        }

        static List<int[]> findRightLeft(int x0, int y0){
            List<int[]> sequence = new List<int[]>{
                new int[] {x0, y0}
            };
            int n = 0;
            bool[] visited = new bool[9] {false, false, false, false, false, false, false, false, false};
            visited[x0] = true;

            while(n < sequence.Count){
                int x = sequence[n][0];
                int y = sequence[n][1];
                if(x > 0 && y < 8 && ball_map[x-1,y+1]==ball_map[x0,y0] && !visited[x-1]){
                    sequence.Add(new int[]{x-1,y+1});
                    visited[x-1] = true;
                }
                if(x < 8 && y > 0 && ball_map[x+1,y-1]==ball_map[x0,y0] && !visited[x+1]){
                    sequence.Add(new int[]{x+1, y-1});
                    visited[x+1] = true;
                }
                n++;
            }

            return sequence;
        }

        static int foundSequence(List<int[]> sequence){
            int score = 0;
            if(sequence.Count >= 5){
                score += sequence.Count;
                foreach(int[] location in sequence){
                    ball_map[location[0], location[1]] = 0;
                }
            }
            return score;
        }

        static int scoring(int x0, int y0){
            int score = 0;
            List<int[]> vertical = findVertical(x0, y0);
            List<int[]> horizontal = findHorizontal(x0, y0);
            List<int[]> leftright = findLeftRight(x0, y0);
            List<int[]> rightleft = findRightLeft(x0, y0);

            score += foundSequence(vertical);
            score += foundSequence(horizontal);
            score += foundSequence(leftright);
            score += foundSequence(rightleft);
            return score;
        }

        public static int proceed_next(){
            // Handling scoring that may happen after showing the predict balls
            int score = 0;
            for(int i=0; i<predict; i++){
                score += scoring(next_balls[i, 1], next_balls[i, 2]);
            }
            return score;
        }

        public static (bool, int) move(int x_begin, int y_begin, int x_end, int y_end){
            if(ball_map[x_end, y_end] != 0){
                return (false, 0);
            }

            Queue<int[]> adjacent = new Queue<int[]>();
            adjacent.Enqueue(new int[] {x_begin, y_begin});
            bool valid = false;
            bool[,] visited = {
                {false, false, false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false, false, false},
            };
            visited[x_begin, y_begin] = true;
            while(adjacent.Count != 0){
                int[] pos = adjacent.Dequeue();
                int x = pos[0];
                int y = pos[1];

                if(x == x_end && y == y_end){
                    valid = true;
                    break;
                }
                if(x > 0 && ball_map[x-1,y] == 0 && !visited[x-1, y]){
                    adjacent.Enqueue(new int[] {x-1, y});
                    visited[x-1, y] = true;
                }
                if(y > 0 && ball_map[x, y-1] == 0 && !visited[x, y-1]){
                    adjacent.Enqueue(new int[] {x, y-1});
                    visited[x, y-1] = true;
                }
                if(x < 8 && ball_map[x+1, y] == 0 && !visited[x+1, y]){
                    adjacent.Enqueue(new int[] {x+1, y});
                    visited[x+1, y] = true;
                }
                if(y < 8 && ball_map[x, y+1] == 0 && !visited[x, y+1]){
                    adjacent.Enqueue(new int[] {x, y+1});
                    visited[x, y+1] = true;
                }
            }
            if(valid){
                ball_map[x_end, y_end] = ball_map[x_begin, y_begin];
                ball_map[x_begin, y_begin] = 0;
                // Check if the move scores
                int score = scoring(x_end, y_end);
                return (true, score);
            }else{
                Debug.Log("Invalid move");
                return (false, 0);
            }
        }

        public static bool game_end(){
            int empty = 0;
            for(int i=0; i<9; i++){
                for(int j=0; j<9; j++){
                    if(ball_map[i,j] == 0){
                        empty++;
                    }
                }
            }
            if(empty <= 3){
                return true;
            }
            return false;
        }
    }
}