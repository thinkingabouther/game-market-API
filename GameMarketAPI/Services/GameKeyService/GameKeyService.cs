using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using game_market_API.DTOs;
using game_market_API.ExceptionHandling;
using game_market_API.Models;
using game_market_API.Utilities;
using game_market_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public class GameKeyService : IGameKeyService
    {
        private readonly GameMarketDbContext _context;
        private IMapper _mapper;

        public GameKeyService(GameMarketDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameKeyViewModel>> GetGameKeysAsync(string vendorUserName)
        {
            var data= await _context.GameKeys
                .Include(key => key.Game)
                .ThenInclude(g => g.Vendor)
                .Where(key => key.Game.Vendor.Username == vendorUserName)
                .ToListAsync();
            if (data.Count == 0) throw new ItemNotFoundException();
            return _mapper.Map<IEnumerable<GameKey>, IEnumerable<GameKeyViewModel>>(data);
        }

        public async Task<GameKeyViewModel> GetGameKeyAsync(string vendorUserName, int id)
        {
            var gameKey = await Task.Run(() => _context.GameKeys
                .Include(key => key.Game)
                .ThenInclude(g => g.Vendor)
                .Single(key => key.ID == id));
            if (gameKey == null) throw new ItemNotFoundException();
            if (gameKey.Game.Vendor.Username != vendorUserName) throw new WrongVendorException();
            return _mapper.Map<GameKeyViewModel>(gameKey);
        }

        public async Task<GameKeyViewModel> PostGameKeyAsync(string vendorUserName, GameKeyDto gameKeyDto)
        {
            var gameKey = MapToEntity(vendorUserName, gameKeyDto);
            _context.GameKeys.Add(gameKey);
            await _context.SaveChangesAsync();
            return _mapper.Map<GameKeyViewModel>(gameKey);
        }

        public async Task<ActionResult<GameKeyViewModel>> DeleteGameKey(string vendorUserName, int id)
        {
            var gameKey = await Task.Run(() => _context.GameKeys
                .Include(key => key.Game)
                .ThenInclude(g => g.Vendor)
                .Single(key => key.ID == id));
            if (gameKey == null) throw new ItemNotFoundException();
            if (gameKey.Game.Vendor.Username != vendorUserName) throw new WrongVendorException();
            _context.GameKeys.Remove(gameKey);
            await _context.SaveChangesAsync();
            return _mapper.Map<GameKeyViewModel>(gameKey);
        }

        private GameKey MapToEntity(string vendorUserName, GameKeyDto dto)
        {
            var game = _context.Games.Include(game => game.Vendor).Single(game => game.ID == dto.GameID);
            if (game == null) throw new ItemNotFoundException();
            if (game.Vendor.Username != vendorUserName) throw new WrongVendorException();
            return new GameKey
            {
                Key = dto.Key,
                Game = game,
            };
        }
    }
}