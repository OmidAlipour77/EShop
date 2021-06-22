using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class UnitOfWork : IDisposable
    {
        MyShop_DBEntities db = new MyShop_DBEntities();
        private GenericRepository<Users> userRepository;
        private GenericRepository<Roles> roleRepository;
        private GenericRepository<ProductGroups> productGroupsRepository;
        private GenericRepository<Products> productRepository;
        private GenericRepository<ProductSelectGroup> selectGroupRepository;
        private GenericRepository<ProductTags> tagsRepository;
        private GenericRepository<Product_Galleries> galleryRepository;
        private GenericRepository<Feature> featureRepository;
        private GenericRepository<ProductFeature> productFeatureRepository;
        private GenericRepository<ProductComment> productCommentRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<OrderDetails> orderDitailsRepository;
        public GenericRepository<Users> UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new GenericRepository<Users>(db);
                }
                return userRepository;
            }
        }
        public GenericRepository<Roles> RoleRepository
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new GenericRepository<Roles>(db);
                }
                return roleRepository;
            }
        }
        public GenericRepository<ProductGroups> ProductGroupsRepository
        {
            get
            {
                if (productGroupsRepository == null)
                {
                    productGroupsRepository = new GenericRepository<ProductGroups>(db);
                }
                return productGroupsRepository;
            }
        }
        public GenericRepository<Products> ProductRepository { 
            get {
            if(productRepository==null)
                {
                    productRepository = new GenericRepository<Products>(db);
                }
                return productRepository;
            } }
        public GenericRepository<ProductSelectGroup> SelectGroupRepository { get { 
            if(selectGroupRepository==null)
                {
                    selectGroupRepository = new GenericRepository<ProductSelectGroup>(db);
                }
                return selectGroupRepository;
            } }
        public GenericRepository<ProductTags> TagsRepository { get
            {
                if(tagsRepository==null)
                {
                    tagsRepository = new GenericRepository<ProductTags>(db);
                }
                return tagsRepository;
            } }
        public GenericRepository<Product_Galleries> GalleryRepository {
            get
            {
                if(galleryRepository==null)
                {
                    galleryRepository = new GenericRepository<Product_Galleries>(db);
                }
                return galleryRepository;
            }}
        public GenericRepository<Feature> FeatureRepository
        {
            get
            {
                if (featureRepository == null)
                {
                    featureRepository = new GenericRepository<Feature>(db);
                }
                return featureRepository;
            }
        }
        public GenericRepository<ProductFeature> ProductFeatureRepository
        {
            get
            {
                if (productFeatureRepository == null)
                {
                    productFeatureRepository = new GenericRepository<ProductFeature>(db);
                }
                return productFeatureRepository;
            }
        }
        public GenericRepository<ProductComment> ProductCommentRepository { get {
            if(productCommentRepository==null)
                {
                    productCommentRepository = new GenericRepository<ProductComment>(db);
                }
                return productCommentRepository;
            } }
        public GenericRepository<Order> OrderRepository { get
            {
                if(orderRepository==null)
                {
                    orderRepository = new GenericRepository<Order>(db);
                }
                return orderRepository;
            } }
        public GenericRepository<OrderDetails> OrderDetailsRepository { get {
            if(orderDitailsRepository==null)
                {
                    orderDitailsRepository = new GenericRepository<OrderDetails>(db);
                }
                return orderDitailsRepository;
            } }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
