using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using BusinessLogic.Entities;

namespace BusinessLogic.Specifications
{
    public class BotUserSpecs
    {
        public class GetAll : Specification<BotUser>
        {
            public GetAll() => Query
                .Include(u => u.Purchases)
                .Where(u => true);
        }

        public class GetById : Specification<BotUser>
        {
            public GetById(int id) => Query
                .Include(u => u.Purchases)
                .Where(u => u.Id == id);
        }

        public class GetByChatId : Specification<BotUser>
        {
            public GetByChatId(long chatId) => Query
                .Include(u => u.Purchases)
                .Where(u => u.ChatId == chatId);
        }
    }
}
