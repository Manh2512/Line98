using System;
using UnityEngine;

namespace Utilities{
    public static class BoardPosition{
        public static Vector2 FromCoorToIndex(Vector2 mousePosition, Vector2 size, Vector3 center){
            // Function to get location of mouse click
            float X_left = center.x - size.x/2;
            float Y_top = center.y + size.y/2;

            // Parameters of the boards
            float spacing = 0.04f;
            float width = 0.5f;
            float height = 0.6f;

            // Get index of the click
            float x_pos = (int)Math.Floor((mousePosition.x - X_left) / (spacing + width));
            float y_pos = (int)Math.Floor((Y_top - mousePosition.y) / (spacing + height));
            if(x_pos < 0 || y_pos < 0 || x_pos > 8 || y_pos > 8){
                throw new Exception("Something went wrong!");
            }
            return new Vector2(y_pos, x_pos); // i, j
        }

        public static Vector2 FromIndexToCoor(Vector3 pos0, int i, int j){
            // Parameters of the boards
            float spacing = 0.04f;
            float width = 0.5f;
            float height = 0.6f;

            float x0 = 5*(spacing + width) - width/2f;
            float y0 = 5*(spacing + height) - height/2f;

            float x = spacing + width/2f + (spacing + width)*j; // j corresponding to x
            float y = spacing + height/2f + (spacing + height)*(8-i);   //i corresponding to y

            return new Vector2(x-x0+pos0.x, y-y0+pos0.y);
        }
    }
}

