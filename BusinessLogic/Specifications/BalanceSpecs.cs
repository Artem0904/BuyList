using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using BusinessLogic.Entities;

namespace BusinessLogic.Specifications
{
    public class BalanceSpecs
    {
        public class GetAll : Specification<Balance>
        {
            public GetAll() => Query
                .Include(o => o.User)
                .Where(x => true);
        }
        public class GetById : Specification<Balance>
        {
            public GetById(int id) => Query
                .Include(o => o.User)
                .Where(o => o.Id == id);
        }
        public class GetByUserId : Specification<Balance>
        {
            public GetByUserId(int userId) => Query
                .Include(o => o.User)
                .Where(o => o.UserId == userId);
        }
    }
}
