﻿using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Common.Domain;
using Coolector.Services.Users.Domain;
using Coolector.Services.Users.Queries;
using MongoDB.Driver;
using Coolector.Common.Mongo;
using Coolector.Services.Users.Repositories.Queries;

namespace Coolector.Services.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> ExistsAsync(string name)
            => await _database.Users().ExistsAsync(name);

        public async Task<Maybe<User>> GetByUserIdAsync(string userId)
            => await _database.Users().GetByUserIdAsync(userId);

        public async Task<Maybe<User>> GetByEmailAsync(string email)
            => await _database.Users().GetByEmailAsync(email);

        public async Task<Maybe<User>> GetByNameAsync(string name)
            => await _database.Users().GetByNameAsync(name);

        public async Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query)
        {
            return await _database.Users()
                .Query(query)
                .PaginateAsync(query);
        }

        public async Task AddAsync(User user)
            => await _database.Users().InsertOneAsync(user);

        public async Task UpdateAsync(User user)
            => await _database.Users().ReplaceOneAsync(x => x.Id == user.Id, user);
    }
}