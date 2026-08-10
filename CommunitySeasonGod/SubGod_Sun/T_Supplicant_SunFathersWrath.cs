using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{ 
    class T_Supplicant_SunFathersWrath : Trait
    {

        public static double devastationInLocation = 75;
        public static double devastationInNeighbours = 25;
        public static int damage = 5;

        public override string getName()
        {
            return "Sun-Father's Wrath";
        }

        public override string getDesc()
        {
            return "When this person dies, its location suffers " + devastationInLocation + "% Devastation, and its neighbours suffer " + devastationInNeighbours + "% Devastation. Any heroes, acolytes and agents in the person's location take " + damage +" damage, negated by defence.";
        }

        public override void onDeath(Unit unit, Person killer)
        {
            base.onDeath(unit, killer);

            int totalDamageDealt = 0;
            List<Person> casualties = new List<Person>();

            List<Unit> toKill = new List<Unit>();

            foreach (Unit u in unit.location.units)
            {
                if (u != unit && u is UA ua)
                {
                    if (ua.defence < damage)
                    {
                        int damageDealt = damage - ua.defence;
                        totalDamageDealt += damageDealt;
                        u.hp -= damageDealt;
                        if (u.hp <= 0)
                        {
                            toKill.Add(u);
                        }
                    }
                }
            }

            foreach (Unit u in toKill)
            {
                u.die(unit.map, "Burned by the Solar Patriarch's rage", unit.person);
                if (u.person != null)
                    casualties.Add(u.person);
            }

            if (unit.location.isOcean == false)
            {
                Property.addToProperty("Sun-Father's Wrath", Property.standardProperties.DEVASTATION, devastationInLocation, unit.location);
            }
            foreach (Location l in unit.location.getNeighbours())
            {
                if (l.isOcean == false)
                {
                    Property.addToProperty("Sun-Father's Wrath", Property.standardProperties.DEVASTATION, devastationInNeighbours, l);
                }
            }

            if (casualties.Count == 0) 
                unit.map.addUnifiedMessage(unit, null, "Sun-Father's Wrath", "The Solar Patriarch, furious at the death of " + unit.getName() + ", engulfs the land around their corpse in flame. \n\nOther characters at the location suffered a total of " + totalDamageDealt + " damage.", "SUN-FATHER'S WRATH");
            else if (casualties.Count == 1)
                unit.map.addUnifiedMessage(unit, casualties[0], "Sun-Father's Wrath", "The Solar Patriarch, furious at the death of " + unit.getName() + ", engulfs the land around their corpse in flame. \n\n" + casualties[0].getName() + " died in the blast.", "SUN-FATHER'S WRATH");
            else if (casualties.Count == 2)
                unit.map.addUnifiedMessage(unit, casualties[0], "Sun-Father's Wrath", "The Solar Patriarch, furious at the death of " + unit.getName() + ", engulfs the land around their corpse in flame. \n\n" + casualties[0].getName() + " and one other character died in the blast.", "SUN-FATHER'S WRATH");
            else
                unit.map.addUnifiedMessage(unit, casualties[0], "Sun-Father's Wrath", "The Solar Patriarch, furious at the death of " + unit.getName() + ", engulfs the land around their corpse in flame. \n\n" + casualties[0].getName() + " and " + casualties.Count() + " other characters died in the blast.", "SUN-FATHER'S WRATH");


            //Volcanic devastation effect
            Hex hex = unit.location.hex;
            Hex[][] array = World.staticMap.grid[0];
            foreach (Hex[] array2 in array)
            {
                Hex[] array3 = array2;
                foreach (Hex hex2 in array3)
                {
                    double num3 = Math.Sqrt((hex.x - hex2.x) * (hex.x - hex2.x) + (hex.y - hex2.y) * (hex.y - hex2.y));
                    if (!(num3 < (double)(2 + Eleven.random.Next(2))))
                    {
                        continue;
                    }

                    hex2.volcanicDamage = (int)((double)(unit.map.param.mg_volcanicBaseEffect + Eleven.random.Next(unit.map.param.mg_volcanicBaseRand)) - num3 * (double)unit.map.param.mg_volcanicBaseDist);
                    World.log("Volcanic effect applied " + hex2.x + ", " + hex2.y);
                    if (num3 >= 1.0 && Eleven.random.Next(3) != 0)
                    {
                        hex2.isMountain = true;
                    }

                    if (hex2.location == null)
                    {
                        continue;
                    }
                }
            }
        }
    }
}