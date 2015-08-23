using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Text;

namespace AscianXbox
{
    public static class ControllerInput
    {
        static KeyboardState keypressed;

        public static void handleCommand(ref Character character, ref Playable playable, ref global_vars variables, ref Game1 game)
        {
            keypressed = Keyboard.GetState();

            //Player movement
     
            if (keypressed.IsKeyDown(variables.CHARACTER_UP) && (character.getPosition().Y > 0))
            {
                character.setPosition(character.getPosition().X, character.getPosition().Y - variables.CHARACTER_DEFAULT_MOVE_SPEED);
                character.move_state = global_vars.movestate.up;
            }
            else if (keypressed.IsKeyDown(variables.CHARACTER_DOWN) && (character.getPosition().Y < (variables.screen_height - variables.CHARACTER_HEIGHT)))
            {
                character.setPosition(character.getPosition().X, character.getPosition().Y + variables.CHARACTER_DEFAULT_MOVE_SPEED);
                character.move_state = global_vars.movestate.down;
            }
            else if (keypressed.IsKeyDown(variables.CHARACTER_RIGHT) && (character.getPosition().X < variables.screen_width - variables.CHARACTER_WIDTH))
            {
                character.setPosition(character.getPosition().X + variables.CHARACTER_DEFAULT_MOVE_SPEED, character.getPosition().Y);
                character.move_state = global_vars.movestate.right;
            }
            else if (keypressed.IsKeyDown(variables.CHARACTER_LEFT) && (character.getPosition().X > 0))
            {
                character.setPosition(character.getPosition().X - variables.CHARACTER_DEFAULT_MOVE_SPEED, character.getPosition().Y);
                character.move_state = global_vars.movestate.left;
            }

            else character.move_state = global_vars.movestate.notmoving;

            //User commands (spells, shields, etc)
            //Attacks start at player's center
            int posx = character.getPosition().X + (variables.CHARACTER_WIDTH / 4);
            int posy = character.getPosition().Y + (variables.CHARACTER_HEIGHT / 4);
            int shieldx = character.getPosition().X - (variables.SHIELD_WIDTH / 2);
            int shieldy = character.getPosition().Y - (variables.SHIELD_HEIGHT / 2);

            //Sheild (One at a time)
            if (keypressed.IsKeyDown(variables.SHIELD) && (character.shieldOut == false))
            {
                try
                {
                    bool answer = playable.notify(global_vars.notification.ShieldRequest);
                    if (answer)
                    {
                        character.shieldOut = true;
                        game.Components.Add(new Basic_Shield(10000, new Rectangle(shieldx, shieldy, variables.SHIELD_WIDTH, variables.SHIELD_HEIGHT), ref character, ref variables, ref game));
                    }
                }
                catch (Exception e) { throw e; }
            }

            //Basic Attack
            if (keypressed.IsKeyDown(variables.ATTACK_BASIC_UP) && (character.attackReady == true))
            {
                character.attackReady = false;
                game.Components.Add(new Basic_Attack(3000, new Rectangle(posx, posy, variables.ATTACK_SPELL_SIZE, variables.ATTACK_SPELL_SIZE), 'u', global_vars.sender.Character, ref variables, ref game));
            }
            else if (keypressed.IsKeyDown(variables.ATTACK_BASIC_RIGHT) && (character.attackReady == true))
            {
                character.attackReady = false;
                game.Components.Add(new Basic_Attack(3000, new Rectangle(posx, posy, variables.ATTACK_SPELL_SIZE, variables.ATTACK_SPELL_SIZE), 'r', global_vars.sender.Character, ref variables, ref game));
            }
            else if (keypressed.IsKeyDown(variables.ATTACK_BASIC_LEFT) && (character.attackReady == true))
            {
                character.attackReady = false;
                game.Components.Add(new Basic_Attack(3000, new Rectangle(posx, posy, variables.ATTACK_SPELL_SIZE, variables.ATTACK_SPELL_SIZE), 'l', global_vars.sender.Character, ref variables, ref game));
            }
            else if (keypressed.IsKeyDown(variables.ATTACK_BASIC_DOWN) && (character.attackReady == true))
            {
                character.attackReady = false;
                game.Components.Add(new Basic_Attack(3000, new Rectangle(posx, posy, variables.ATTACK_SPELL_SIZE, variables.ATTACK_SPELL_SIZE), 'd', global_vars.sender.Character, ref variables, ref game));
            }

            //Special Ability
            if (keypressed.IsKeyDown(variables.SPECIAL_ABILITY))
            {
                try
                {
                    bool answer = playable.notify(global_vars.notification.SpecialRequest);
                    if (answer)
                    {
                        Ability_Manager.performSpecialAbility(character.getSpecial(), ref variables, ref game);
                    }
                }
                catch (Exception e) { }
            }
            
            //Exit Full Screen
           // if (keypressed.IsKeyDown(Keys.Escape))
          //      variables.manager.ToggleFullScreen();
        }

        public static void handleMenuCommands(Main_Menu main)
        {
            keypressed = Keyboard.GetState();
            if (keypressed.IsKeyDown(Keys.Down))
                main.NextOption();
            else if (keypressed.IsKeyDown(Keys.Up))
                main.PreviousOption();
            else if (keypressed.IsKeyDown(Keys.Enter))
                main.SelectOption();
       
        }

        public static void handleUpgradeScreenCommands(ref Upgrade upgrade, ref global_vars vars)
        {
            keypressed = Keyboard.GetState();
            if (keypressed.IsKeyDown(vars.PURCHASE))
                upgrade.buyAbility();
            else if (keypressed.IsKeyDown(vars.SELECT))
                upgrade.setAbility();
            else if (keypressed.IsKeyDown(vars.REMOVE))
                upgrade.remove();
            else if (keypressed.IsKeyDown(Keys.Right))
                upgrade.moveAbilityRight();
            else if (keypressed.IsKeyDown(Keys.Left))
                upgrade.moveAbilityLeft();
            else if (keypressed.IsKeyDown(Keys.Escape))
                upgrade.quit();
        }

        public static void handlePauseScreenCommands(ref Pause pause, ref global_vars vars)
        {
            keypressed = Keyboard.GetState();
            if (keypressed.IsKeyDown(vars.PAUSE))
                pause.PauseGame();
            else if (keypressed.IsKeyDown(vars.QUIT))
                pause.Quit();

        }

        public static void handleTargetCommands(ref Target target, ref global_vars vars)
        {
            keypressed = Keyboard.GetState();
            if (keypressed.IsKeyDown(Keys.Up))
                target.move('u');
            else if (keypressed.IsKeyDown(Keys.Down))
                target.move('d');
            else if (keypressed.IsKeyDown(Keys.Right))
                target.move('r');
            else if (keypressed.IsKeyDown(Keys.Left))
                target.move('l');
            else if (keypressed.IsKeyDown(Keys.Enter))
                target.select();
        }

        public static void handleCreditCommands(ref Credits credit)
        {
            keypressed = Keyboard.GetState();
            if (keypressed.IsKeyDown(Keys.Escape))
                credit.exit();
        }
    }
}
