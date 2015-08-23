using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace AscianXbox
{
    public static class Collision_Detector
    {
        static List<Shield> shields = new List<Shield>();
        static List<Spell> spells = new List<Spell>();
        static List<Special> specials = new List<Special>();
        static List<Enemy> enemies = new List<Enemy>();
        static List<Character> characters = new List<Character>();

        static List<Spell> spellsToRemove = new List<Spell>();
        static List<Enemy> enemiesToRemove = new List<Enemy>();
        static List<Character> charactersToRemove = new List<Character>();

        public static void removeAll()
        {
            shields.Clear();
            spells.Clear();
            specials.Clear();
            enemies.Clear();
            characters.Clear();
            spellsToRemove.Clear();
            enemiesToRemove.Clear();
            charactersToRemove.Clear();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addShield(ref Shield shield)
        {
            lock (shields)
            {
                shields.Add(shield);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void removeShield(ref Shield shield)
        {
            lock (shields)
            {
                shields.Remove(shield);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addSpecial(ref Special special)
        {
            lock (specials)
            {
                specials.Add(special);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void removeSpecial(ref Special special)
        {
            lock (specials)
            {
                specials.Remove(special);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addSpell(ref Spell spell)
        {
            lock (spells)
            {
                spells.Add(spell);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void removeSpell(Spell spell)
        {
            lock (spellsToRemove)
            {
                spellsToRemove.Add(spell);
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addEnemy(ref Enemy enemy)
        {
            lock (enemies)
            {
                enemies.Add(enemy);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void removeEnemy(Enemy enemy)
        {
            lock (enemiesToRemove)
            {
                enemiesToRemove.Add(enemy);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addCharacter(ref Character character)
        {
            lock (characters)
            {
                characters.Add(character);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void removeCharacter(Character character)
        {
            lock (charactersToRemove)
            {
                charactersToRemove.Add(character);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        static void removeCollidedObjects()
        {
            lock (spells)
            {
                foreach (Spell s in spellsToRemove)
                {
                    spells.RemoveAll(Spell => Spell == s);
                    s.kill();
                }
            }
            lock (enemies)
            {
                foreach (Enemy e in enemiesToRemove)
                {
                    enemies.RemoveAll(Enemy => Enemy == e);
                    e.kill();
                }
            }
            lock (characters)
            {
                foreach (Character c in charactersToRemove)
                {
                    characters.RemoveAll(Character => Character == c);
                    c.kill();
                }
            }
            //Clear Collided Objects Lists
            spellsToRemove.Clear();
            enemiesToRemove.Clear();
            charactersToRemove.Clear();
        }

        public static void checkCollisions()
        {
            checkEnemyCollision();
            checkCharacterCollision();
            checkMiscCollision();
            removeCollidedObjects();
        }

        //New Optimized Enemy Collision
        static void checkEnemyCollision()
        {
            Rectangle shpos;
            Rectangle cpos;
            Rectangle sppos;
            Rectangle specialPos;

            lock (enemies)
            {
                foreach (Enemy e in enemies)
                {
                    Rectangle epos = e.getPosition();

                    //Enemy Colliding with Spells
                    if (spells.Count != 0)
                    {
                        foreach (Spell s in spells)
                        {
                            sppos = s.getPosition();
                            if (sppos.Intersects(epos) && s.getSender().Equals(global_vars.sender.Character))
                            {
                                removeSpell(s);
                                removeEnemy(e);
                                break;
                            }
                        }
                    }
                    //Enemy Colliding with Specials
                    if (specials.Count != 0)
                    {
                        foreach (Special sp in specials)
                        {
                            specialPos = sp.getPosition();
                            if (specialPos.Intersects(epos))
                            {
                                removeEnemy(e);
                                break;
                            }
                        }
                    }
                    //Enemy Colliding with Characters
                    foreach (Character c in characters)
                    {
                        cpos = c.getPosition();
                        if (cpos.Intersects(epos))
                        {
                            if (epos.Intersects(new Rectangle(cpos.X + (cpos.Width / 2), cpos.Y + (cpos.Height / 2), 1, 1)))
                                removeCharacter(c);
                            break;
                        }
                    }
                    //Enemy Colliding with Shields
                    if (shields.Count != 0)
                    {
                        foreach (Shield s in shields)
                        {
                            shpos = s.getPosition();
                            if (shpos.Intersects(epos))
                            {
                                removeEnemy(e);
                                break;
                            }
                        }
                    }
                }
            }

        }
  
        static void checkCharacterCollision()
        {
            Rectangle spos;
            Rectangle cpos;

            foreach (Character c in characters)
            {
                //Check Character Collisions with Spells
                cpos = c.getPosition();
               foreach (Spell s in spells)
                {
                    spos = s.getPosition();
                    if (cpos.Intersects(spos) && s.getSender().Equals(global_vars.sender.Enemy))
                    {
                        if (spos.Intersects(new Rectangle(cpos.X + (cpos.Width / 2), cpos.Y + (cpos.Height / 2), 1, 1)))
                        {
                            removeCharacter(c);
                            removeSpell(s);
                        }
                    }
                }
            }
        }

        static void checkMiscCollision()
        {
            Rectangle shpos;
            Rectangle sppos;
            //Check enemy spells colliding with shields
            foreach (Shield sh in shields)
            {
                shpos = sh.getPosition();
                foreach (Spell spell in spells)
                {
                    sppos = spell.getPosition();
                    if(spell.getSender().Equals(global_vars.sender.Enemy) && sppos.Intersects(shpos))
                    {
                        spell.kill();
                    }
                }
            }

        }

        public static List<Enemy> getEnemiesCollided(Rectangle collideWith)
        {
            List<Enemy> list = new List<Enemy>(enemies.Count);
            foreach (Enemy e in enemies)
            {
                if (e.getPosition().Intersects(collideWith))
                    list.Add(e);
            }
            return list;
        }
     
    }
}
