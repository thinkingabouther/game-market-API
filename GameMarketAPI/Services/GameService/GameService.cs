using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.Utilities;
using game_market_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public class GameService : IGameService
    {
        private readonly GameMarketDbContext _context;

        private readonly IMapper _mapper;

        public GameService(GameMarketDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameViewModel>> GetGamesAsync()
        {
            var data = _context.Games
                .Include(g => g.Vendor)
                .Include(g => g.GameKeys)
                .ToListAsync();
            return _mapper.Map<IEnumerable<Game>, IEnumerable<GameViewModel>>(await data);
        }

        public async Task<GameViewModel> GetGameAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) throw new ItemNotFoundException();
            return _mapper.Map<GameViewModel>(game);
        }
        
        
        public async Task PatchGameAsync(string vendorUserName, int id, GameDto game)
        {
            var gameInDB = _context.Games.Include("Vendor").Single(game => game.ID == id);
            if (gameInDB.Vendor.Username != vendorUserName) throw new WrongVendorException();
            gameInDB.Name = game.Name;
            gameInDB.Price = game.Price;
            _context.Entry(gameInDB).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    throw new ItemNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }
        
        
        public async Task<GameViewModel> PostGameAsync(string vendorUserName, Game game)
        {
            var vendor = _context.Users.Single(u => u.Username == vendorUserName);
            game.Vendor = vendor;
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return _mapper.Map<GameViewModel>(game);
        }

        public async Task<GameViewModel> DeleteGame(string vendorUserName, int id)
        {
            var game = _context.Games.Include("Vendor").Single(game => game.ID == id);
            if (game == null)
            {
                throw new ItemNotFoundException();
            }
            if (game.Vendor.Username != vendorUserName) throw new WrongVendorException();
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return _mapper.Map<GameViewModel>(game);
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.ID == id);
        }
    }
}