using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheMaze.Entities;
using Microsoft.Xna.Framework;

namespace TheMaze
{
    class PathFinder
    {
        #region singleton thread safe
        private static readonly PathFinder instance = new PathFinder();
        private PathFinder() { }
        static PathFinder()
        {
            instance.closedList = new List<Platform>();
            instance.OpenedList = new List<Platform>();
        }

        public static PathFinder Instance
        {
            get { return instance; }
        }
        #endregion singleton

        List<Platform> closedList;
        List<Platform> OpenedList;

        public List<Platform> AvailablePlatforms(Platform i_plateform)
        {
            List<Platform> available_platform = new List<Platform>();
            OpenedList.Add(i_plateform);
            available_platform.Add(i_plateform);

            while (OpenedList.Count != 0)
            {
                Platform platform_origin = OpenedList.First<Platform>();

                // on test si les platforms adjacentes sont accessible
                // si oui, on les ajoute dans available_platform & OpenList
                Platform adjacent_platform;
                if (CanMoveUp(platform_origin, out adjacent_platform) == true && !closedList.Contains(adjacent_platform))
                {
                    adjacent_platform.cost = platform_origin.cost + 1;
                    if (!adjacent_platform.floor || 
                        (adjacent_platform.floor && adjacent_platform.position == Vector2.Zero && adjacent_platform.CanMoveLeft() == TileCollision.Passable))
                        available_platform.Add(adjacent_platform);
                    OpenedList.Add(adjacent_platform);
                }
                if (CanMoveDown(platform_origin, out adjacent_platform) == true && !closedList.Contains(adjacent_platform))
                {
                    adjacent_platform.cost = platform_origin.cost + 1;
                    if (!adjacent_platform.floor)
                        available_platform.Add(adjacent_platform);
                    OpenedList.Add(adjacent_platform);
                }
                if (CanMoveRight(platform_origin, out adjacent_platform) == true && !closedList.Contains(adjacent_platform))
                {
                    adjacent_platform.cost = platform_origin.cost + 1;
                    if (!adjacent_platform.floor)
                        available_platform.Add(adjacent_platform);
                    OpenedList.Add(adjacent_platform);
                }
                if (CanMoveLeft(platform_origin, out adjacent_platform) == true && !closedList.Contains(adjacent_platform))
                {
                    adjacent_platform.cost = platform_origin.cost + 1;
                    if (!adjacent_platform.floor || 
                        (adjacent_platform.floor && adjacent_platform.position == Vector2.Zero && adjacent_platform.CanMoveLeft() == TileCollision.Passable))
                        available_platform.Add(adjacent_platform);
                    OpenedList.Add(adjacent_platform);
                }

                // on supprime la platform origin de la liste de recherche
                OpenedList.Remove(platform_origin);
                closedList.Add(platform_origin);
            }

            closedList.Clear();
            OpenedList.Clear();

            return available_platform;
        }

        bool CanMoveUp(Platform i_plateform, out Platform o_adjacent_platform)
        {
            bool ret = false;
            o_adjacent_platform = null;

            if (i_plateform.position.Y - 1 >= 0)
            {
                Vector2 adjacent_position = new Vector2(i_plateform.position.X, i_plateform.position.Y - 1);
                if (LevelManager.Instance.currentLevel.hole_position == adjacent_position) return false;

                o_adjacent_platform = LevelManager.Instance.currentLevel.Plateform(adjacent_position);

                if ((i_plateform.CanMoveUp() == TileCollision.Passable && o_adjacent_platform.CanMoveDown() == TileCollision.Passable && i_plateform.floor == o_adjacent_platform.floor) ||
                    (i_plateform.CanMoveUp() == TileCollision.Escalier && o_adjacent_platform.CanMoveDown() == TileCollision.Passable && o_adjacent_platform.floor == true) ||
                    o_adjacent_platform.CanMoveDown() == TileCollision.Escalier && i_plateform.CanMoveUp() == TileCollision.Passable && i_plateform.floor == true)
                    ret = true;
                else
                {
                    o_adjacent_platform = null;
                }
            }

            return ret;
        }

        bool CanMoveDown(Platform i_plateform, out Platform o_adjacent_platform)
        {
            bool ret = false;
            o_adjacent_platform = null;

            if (i_plateform.position.Y + 1 < 3)
            {
                Vector2 adjacent_position = new Vector2(i_plateform.position.X, i_plateform.position.Y + 1);
                if (LevelManager.Instance.currentLevel.hole_position == adjacent_position) return false;

                o_adjacent_platform = LevelManager.Instance.currentLevel.Plateform(adjacent_position);

                if ((i_plateform.CanMoveDown() == TileCollision.Passable && o_adjacent_platform.CanMoveUp() == TileCollision.Passable && i_plateform.floor == o_adjacent_platform.floor) ||
                    (i_plateform.CanMoveDown() == TileCollision.Escalier && o_adjacent_platform.CanMoveUp() == TileCollision.Passable && o_adjacent_platform.floor == true) ||
                    o_adjacent_platform.CanMoveUp() == TileCollision.Escalier && i_plateform.CanMoveDown() == TileCollision.Passable && i_plateform.floor == true)
                    ret = true;
                else
                {
                    o_adjacent_platform = null;
                }
            }

            return ret;
        }

        bool CanMoveRight(Platform i_plateform, out Platform o_adjacent_platform)
        {
            bool ret = false;
            o_adjacent_platform = null;

            if (i_plateform.position.X + 1 < 3)
            {
                Vector2 adjacent_position = new Vector2(i_plateform.position.X + 1, i_plateform.position.Y);
                if (LevelManager.Instance.currentLevel.hole_position == adjacent_position) return false;

                o_adjacent_platform = LevelManager.Instance.currentLevel.Plateform(adjacent_position);

                if ((i_plateform.CanMoveRight() == TileCollision.Passable && o_adjacent_platform.CanMoveLeft() == TileCollision.Passable && i_plateform.floor == o_adjacent_platform.floor) ||
                    (i_plateform.CanMoveRight() == TileCollision.Escalier && o_adjacent_platform.CanMoveLeft() == TileCollision.Passable && o_adjacent_platform.floor == true) ||
                    o_adjacent_platform.CanMoveLeft() == TileCollision.Escalier && i_plateform.CanMoveRight() == TileCollision.Passable && i_plateform.floor == true)
                    ret = true;
                else
                {
                    o_adjacent_platform = null;
                }
            }

            return ret;
        }

        bool CanMoveLeft(Platform i_plateform, out Platform o_adjacent_platform)
        {
            bool ret = false;
            o_adjacent_platform = null;

            if (i_plateform.position.X - 1 >= 0)
            {
                Vector2 adjacent_position = new Vector2(i_plateform.position.X - 1, i_plateform.position.Y);
                if (LevelManager.Instance.currentLevel.hole_position == adjacent_position) return false;

                o_adjacent_platform = LevelManager.Instance.currentLevel.Plateform(adjacent_position);

                if ((i_plateform.CanMoveLeft() == TileCollision.Passable && o_adjacent_platform.CanMoveRight() == TileCollision.Passable && i_plateform.floor == o_adjacent_platform.floor) ||
                    (i_plateform.CanMoveLeft() == TileCollision.Escalier && o_adjacent_platform.CanMoveRight() == TileCollision.Passable && o_adjacent_platform.floor == true) ||
                    o_adjacent_platform.CanMoveRight() == TileCollision.Escalier && i_plateform.CanMoveLeft() == TileCollision.Passable && i_plateform.floor == true)
                    ret = true;
                else
                {
                    o_adjacent_platform = null;
                }
            }

            return ret;
        }
    }
}
