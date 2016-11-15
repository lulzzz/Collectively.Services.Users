﻿using Coolector.Services.Users.Domain;
using Coolector.Services.Users.Queries;
using Coolector.Services.Users.Services;

namespace Coolector.Services.Users.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(IUserService userService) : base("users")
        {
            Get("", async args => await FetchCollection<BrowseUsers, User>
                (async x => await userService.BrowseAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetUser, User>
                (async x => await userService.GetAsync(x.Id)).HandleAsync());

            Get("{name}/account", async args => await Fetch<GetUserByName, User>
                (async x => await userService.GetByNameAsync(x.Name)).HandleAsync());

            Get("{name}/available", async args => await Fetch<GetNameAvailability, dynamic>
                (async x => await userService.IsNameAvailableAsync(x.Name)).HandleAsync());
        }
    }
}