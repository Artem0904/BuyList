using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using BusinessLogic.Entities;

namespace BusinessLogic.Specifications
{
    public class OrderSpecs
    {
        public class GetAll : Specification<Purchase>
        {
            public GetAll() => Query
                .Include(o => o.User)
                .Where(x => true);
        }
        public class GetById : Specification<Purchase>
        {
            public GetById(int id) => Query
                .Include(o => o.User)
                .Where(o => o.Id == id);
        }
    }
}
