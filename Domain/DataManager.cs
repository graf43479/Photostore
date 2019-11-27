using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;

namespace Domain
{
    public class DataManager
    {
        private ICategoryRepository categoryRepository;
        private IProductRepository productRepository;
        private IUserRepository userRepository;
        private IDeliveryProcessor deliveryProcessor;
        private PrimaryMembershipProvider provider;
        private PrimaryRoleProvider roleProvider;



        public DataManager(ICategoryRepository categoryRepository, IProductRepository productRepository,
            IUserRepository userRepository, IDeliveryProcessor deliveryProcessor, PrimaryMembershipProvider provider, PrimaryRoleProvider roleProvider)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
            this.provider = provider;
            this.roleProvider = roleProvider;
            this.deliveryProcessor = deliveryProcessor;
        }

        public ICategoryRepository Categories
        {
            get { return categoryRepository; }
        }

        public IProductRepository Products
        {
            get { return productRepository; }
        }


        public IUserRepository UsersRepository
        {
            get { return userRepository; }
        }

        public IDeliveryProcessor DeliveryProcessor
        {
            get { return deliveryProcessor; }
        }

        public PrimaryMembershipProvider Provider
        {
            get { return provider; }
        }

        public PrimaryRoleProvider RoleProvider
        {
            get { return roleProvider; }
        }
    }
}
