using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public struct global_vars
{
    public global_vars(GraphicsDeviceManager m)
    {
        screen_height = m.GraphicsDevice.Viewport.Height;
        screen_width = m.GraphicsDevice.Viewport.Width;

        CHARACTER_HEIGHT = screen_height / 16;
        CHARACTER_WIDTH =  screen_width / 16;
        ENEMY_HEIGHT = screen_height / 32;
        ENEMY_WIDTH = screen_width / 30;
        SHADOW_HEIGHT = screen_height / 16;
        SHADOW_WIDTH = screen_width / 15;
        CHARACTER_UPDATE_PRIORITY = 1;
        ABILITY_UPDATE_PRIORITY = 0;
        ENEMY_UPDATE_PRIORITY = 2;
        CHARACTER_START_POSITION = new Point(screen_width / 2, screen_height / 2);
        CHARACTER_DEFAULT_MOVE_SPEED = 3;
        ENEMY_MOVE_SPEED = 1;
        CHARACTER_DEFAULT_MOVESTATE = movestate.notmoving;

        CHARACTER_UP = Keys.Up;
        CHARACTER_DOWN = Keys.Down;
        CHARACTER_LEFT = Keys.Left;
        CHARACTER_RIGHT = Keys.Right;
        SHIELD = Keys.Space;
        ATTACK_BASIC_UP = Keys.W;
        ATTACK_BASIC_DOWN = Keys.S;
        ATTACK_BASIC_LEFT = Keys.A;
        ATTACK_BASIC_RIGHT = Keys.D;
        SPECIAL_ABILITY = Keys.LeftShift;
        PAUSE = Keys.P;
        QUIT = Keys.Q;
        PURCHASE = Keys.P;
        SELECT = Keys.Enter;
        REMOVE = Keys.R;

        ENEMY_SPAWN_POSITIONS = new Point[12];
        //centers
        ENEMY_SPAWN_POSITIONS[0] = new Point(screen_width /2 , -CHARACTER_HEIGHT);
        ENEMY_SPAWN_POSITIONS[1] = new Point(-CHARACTER_WIDTH, screen_height / 2);
        ENEMY_SPAWN_POSITIONS[2] = new Point(screen_width + CHARACTER_WIDTH, screen_height / 2);
        ENEMY_SPAWN_POSITIONS[3] = new Point(screen_width / 2, screen_height + CHARACTER_HEIGHT);
        //corners
        ENEMY_SPAWN_POSITIONS[4] = new Point(0, 0);
        ENEMY_SPAWN_POSITIONS[5] = new Point(screen_width, screen_height);
        ENEMY_SPAWN_POSITIONS[6] = new Point(screen_width, 0);
        ENEMY_SPAWN_POSITIONS[7] = new Point(0, screen_height);
        //between corners and centers
        ENEMY_SPAWN_POSITIONS[8] = new Point(screen_width / 4, -CHARACTER_HEIGHT);
        ENEMY_SPAWN_POSITIONS[9] = new Point((screen_width / 2) + (screen_width / 4), -CHARACTER_HEIGHT);
        ENEMY_SPAWN_POSITIONS[10] = new Point((screen_width / 4), screen_height + CHARACTER_HEIGHT);
        ENEMY_SPAWN_POSITIONS[11] = new Point((screen_width / 2) + (screen_width / 4), screen_height + CHARACTER_HEIGHT);

        SHIELD_HEIGHT = 200;
        SHIELD_WIDTH = 200;

        ATTACK_SPELL_SIZE = 20;
        ATTACK_SPELL_SPEED = 25;
        ATTACK_DELAY = 250;

        SHADOW_START = 120000;
        SPECIAL_ABILITY_NUMBER = 3;
        HUDLOC = new Vector2(screen_height / 10, screen_width / 10);
        PLOC = new Vector2(100, 70);
        CLOC = new Vector2(100, 160);

        MENU_DELAY = 7;
        PAUSE_DELAY = 1000;
        UPGRADE_DELAY = 100;

        NO_ABILITY = "none";
        ABIL_ONE = "fire";
        ABIL_TWO = "ice";
        ABIL_THREE = "smoke";
        ONE_COST = 100;
        TWO_COST = 100;
        THREE_COST = 100;
        
        manager = m;
    }
    //Graphics Manager for the entire game
    public GraphicsDeviceManager manager;
    //Game Constants
    public readonly int MENU_DELAY;
    public readonly int PAUSE_DELAY;
    public readonly int CHARACTER_HEIGHT;
    public readonly int CHARACTER_WIDTH;

    public readonly int ENEMY_HEIGHT;
    public readonly int ENEMY_WIDTH;
    public readonly int SHADOW_HEIGHT;
    public readonly int SHADOW_WIDTH;


    public readonly int CHARACTER_UPDATE_PRIORITY;     //Priority information used for game to update components.  Low numbers come first
    public readonly int ABILITY_UPDATE_PRIORITY;
    public readonly int ENEMY_UPDATE_PRIORITY;

    public readonly Point CHARACTER_START_POSITION;

    public readonly int CHARACTER_DEFAULT_MOVE_SPEED;

    public readonly movestate CHARACTER_DEFAULT_MOVESTATE;

    public readonly Keys CHARACTER_UP;
    public readonly Keys CHARACTER_DOWN;
    public readonly Keys CHARACTER_LEFT;
    public readonly Keys CHARACTER_RIGHT;
    public readonly Keys SHIELD;
    public readonly Keys ATTACK_BASIC_UP;
    public readonly Keys ATTACK_BASIC_DOWN;
    public readonly Keys ATTACK_BASIC_LEFT;
    public readonly Keys ATTACK_BASIC_RIGHT;
    public readonly Keys SPECIAL_ABILITY;
    public readonly Keys PAUSE;
    public readonly Keys QUIT;
    public readonly Keys PURCHASE;
    public readonly Keys SELECT;
    public readonly Keys REMOVE;

    public readonly Point[] ENEMY_SPAWN_POSITIONS; 
    public readonly int ENEMY_MOVE_SPEED;

    public readonly int SHIELD_HEIGHT;
    public readonly int SHIELD_WIDTH;

    public readonly int ATTACK_SPELL_SIZE;
    public readonly int ATTACK_SPELL_SPEED;
    public readonly int ATTACK_DELAY;

    public readonly Vector2 HUDLOC;
    public readonly Vector2 PLOC;
    public readonly Vector2 CLOC;
    public readonly int SPECIAL_ABILITY_NUMBER;
    public readonly int SHADOW_START;

    public readonly string NO_ABILITY;
    public readonly string ABIL_ONE;
    public readonly string ABIL_TWO;
    public readonly string ABIL_THREE;
    public readonly int ONE_COST;
    public readonly int TWO_COST;
    public readonly int THREE_COST;

    public readonly int UPGRADE_DELAY;

    //Screen Dimensions
    public readonly int screen_height;
    public readonly int screen_width;

    public enum movestate
    {
        notmoving,
        up,
        down,
        left,
        right
    };

    public enum loadType
    {
        Main,
        Campaign,
        Upgrade,
        Credits
    }

    public enum sender
    {
        Character,
        Enemy,
        Special
    }

    public enum notification
    {
        ShieldRequest,
        SpecialRequest,
        PlayerDeath
    }
}