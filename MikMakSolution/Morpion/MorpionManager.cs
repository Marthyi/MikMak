﻿using System;
using System.Linq;
using MikMak.DomainModel.Entities;
using MikMak.Interfaces;
using System.ComponentModel;

namespace Morpion
{
    public class MorpionManager : IGameServices
    {
        public Grid Play(Grid currentState, Move move)
        {            
            //0-Game already finished, no more move
            if (currentState.CurrentMessage.Id == (int)ClassicMessage.GameFinished)
            {
                return currentState;
            }
            //1-Not Your turn
            if (move.PlayerNumber != currentState.NextPlayerToPlay)
            {
                currentState.CurrentMessage = Message.GetMessage(ClassicMessage.NotYourTurn);
                return currentState;
            }
            
            var choosenX = move.Positions[0].Coord.x;
            var choosenY = move.Positions[0].Coord.y;
            //2-Case already taken
            if (currentState.PawnLocations.Where(o => o.Coord.x == choosenX && o.Coord.y == choosenY).Any())
            {
                currentState.CurrentMessage = Message.GetMessage(ClassicMessage.NotEmptyPosition);
                return currentState;
            }
            //3-Case Ok, 
            var PawnToAdd = new Pawn(move.PlayerNumber == 1 ? "B" : "W", choosenX, choosenY);
            currentState.PawnLocations.Add(PawnToAdd);
            currentState.LastMove = move;
            if (IsFinished(currentState, PawnToAdd))
            {
                //3-1 Case Ok + finished
                currentState.NextPlayerToPlay = 0;
                currentState.CurrentMessage = Message.GetMessage(ClassicMessage.GameFinished);
            }
            else
            {
                //3-1 Case Ok + Not finished
                currentState.NextPlayerToPlay = (currentState.NextPlayerToPlay == 1) ? 2 : 1;
                currentState.CurrentMessage = Message.GetMessage((currentState.NextPlayerToPlay == 2) ? MorpionMessage.J2 : MorpionMessage.J1);
            }
            return currentState;
        }

        private bool IsFinished(Grid currentState, Pawn newPawn)
        {
            bool ligneFull = currentState.PawnLocations.Where(o => o.Coord.y == newPawn.Coord.y && o.Name == newPawn.Name).Count() == 3;
            bool colonneFull = currentState.PawnLocations.Where(o => o.Coord.x == newPawn.Coord.x && o.Name == newPawn.Name).Count() == 3;
            bool firstDiag = currentState.PawnLocations.Where(o => o.Coord.IsInFirstDiag() && o.Name == newPawn.Name).Count() == 3;
            bool secondDiag = currentState.PawnLocations.Where(o => o.Coord.IsInSecondDiag() && o.Name == newPawn.Name).Count() == 3;
            return ligneFull || colonneFull || firstDiag || secondDiag;
        }

        public Grid GetNewGame()
        {
            return GetNewGameGridState();         
        }

        private static Grid GetNewGameGridState()
        {
            return new Grid()
            {
                CurrentMessage = Message.GetMessage(MorpionMessage.NewGame),
                IsGridShifted = false,
                MoveNumber = 1,
                NextPlayerToPlay = 1,
                NumberColumns = 3,
                NumberLines = 3                
            };
        }

        public int GetGameType()
        {
            return 1;
        }
    }

    public static class MyExtensionMethods
    {
        public static bool IsInFirstDiag(this Coord point)
        {
            return point.x == point.y;
        }
        public static bool IsInSecondDiag(this Coord point)
        {
            return point.x == 4 - point.y;
        }
    }

    public enum MorpionMessage
    {
        [Description("New game, joueur 1 turn")]
        NewGame = 100,
        [Description("Joueur 1 turn")]
        J1 = 101,
        [Description("Joueur 2 turn")]
        J2 = 102,
    }
}
