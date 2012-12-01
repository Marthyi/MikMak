﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikMak.Interfaces;
using MikMak.Commons;
using MikMak.Main.InternalInterfaces;
using MikMak.Main.Security;

namespace MikMak.Main.GamesManagement
{
    public class GameManager : IGamesManager
    {
        private ITypeGameMapping typeMapping;
        private ILinkPlayersGames linkManager;
        private ISessionManager sessionManager;

        public GameManager()
        {
            // TODO Initialize the field typeMapping and sessionManager. (UNITY)
        }

        public GameManager(IPersistenceManager persistenceManager)
        {
            typeMapping = new TypeGameMappingByReflection();
            linkManager = new LinkPlayersGamesByTextFile();
            sessionManager = new SessionManager(persistenceManager);
        }

        public string GetNewGame(Session initialSession, int gameType, int opponent)
        {
            int playerId = initialSession.PlayerId;
            // 1-Create The game
            var game = typeMapping.GetGame(gameType);
            string gameId = game.GetNewGame();
            // 2-Link the players Id with player numbers
            List<int> listPlayers = new List<int> { initialSession.PlayerId, opponent };
            linkManager.LinkedPlayersToGame(listPlayers, gameId);
            // 3-Create a new Session to be ready to play
            Session newSession = sessionManager.CreateSession(initialSession, gameId, gameType, listPlayers.IndexOf(playerId));
            return newSession.Id;
        }

        public GridState GetState(Session session)
        {
            var game = typeMapping.GetGame(session.GameType);
            return game.GetState(session.GameId);
        }

        public GridState Play(Session session, Move move)
        {
            var game = typeMapping.GetGame(session.GameType);
            return game.Play(session.GameId, move);
        }
    }
}
