using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Mappers;
using BabaFunkeEmailManager.Service.Repositories.Interfaces;
using BabaFunkeEmailManager.Service.Services.Implementations;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BabaFunkeEmailManager.Tests.ServicesFacts
{
    public class SubscriberServiceFacts
    {
        private readonly SubscriberService _sut;
        private readonly Mock<ISubscriberRepository> _repository;
        private readonly Mock<IMapper<Subscriber, SubscriberEntity>> _mapper;
        private readonly List<SubscriberEntity> entities;
        private readonly List<Subscriber> subscribers;

        public SubscriberServiceFacts()
        {
            _repository = new Mock<ISubscriberRepository>();
            _mapper = new Mock<IMapper<Subscriber, SubscriberEntity>>();
            _sut = new SubscriberService(_repository.Object, _mapper.Object);

            entities = new List<SubscriberEntity>
            {
                new SubscriberEntity(){RowKey = "abc@example.com", PartitionKey = "Subscriber"},
                new SubscriberEntity(){RowKey = "def@example.com", PartitionKey = "Subscriber"}
            };

            subscribers = new List<Subscriber>
            {
                new Subscriber(){Email = "abc@example.com"},
                new Subscriber(){Email = "def@example.com"}
            };
        }

        #region GetAllItems
        [Fact, Trait("Category", "GetAllItems")]
        public async Task GetAllItems_ShouldReturnTheRightTotal_WhenNotEmpty()
        {
            const int Total = 2;

            _repository.Setup(r => r.GetAllEntities()).ReturnsAsync(entities);

            _mapper.Setup(m => m.AllEntitiesToModels(entities)).Returns(subscribers);

            var result = await _sut.GetAllItems();

            Assert.Equal(Total, result.Count());
        }

        [Fact, Trait("Category", "GetAllItems")]
        public async Task GetAllItems_ShouldReturnEmptyList_WhenEmpty()
        {
            _repository.Setup(r => r.GetAllEntities()).ReturnsAsync(new List<SubscriberEntity>());

            _mapper.Setup(m => m.AllEntitiesToModels(It.IsAny<List<SubscriberEntity>>())).Returns(new List<Subscriber>());

            var result = await _sut.GetAllItems();

            Assert.Empty(result);
        }

        [Fact, Trait("Category", "GetAllItems")]
        public async Task GetAllItems_ShouldCallTheRepoOnce()
        {
            var result = await _sut.GetAllItems();

            _repository.Verify(r => r.GetAllEntities(), Times.Once());
        }
        #endregion

        #region GetFilteredItems

        [Fact, Trait("Category", "GetFilteredItems")]
        public async Task GetAllActiveSubscribersBySubCategory_ShouldReturnTheFilteredItems_WhenNotEmpty()
        {
            var allEntities = new List<SubscriberEntity>
            {
                new SubscriberEntity(){RowKey = "abc@example.com", PartitionKey = "Subscriber", SubCategory = "Apps", IsSubscribed = true},
                new SubscriberEntity(){RowKey = "def@example.com", PartitionKey = "Subscriber", SubCategory = "Apps", IsSubscribed = true},
                new SubscriberEntity(){RowKey = "ghi@example.com", PartitionKey = "Subscriber", SubCategory = "Books", IsSubscribed = true},
                new SubscriberEntity(){RowKey = "jkl@example.com", PartitionKey = "Subscriber", SubCategory = "Apps", IsSubscribed = false},
            }.Where(e => e.SubCategory == "Apps" && e.IsSubscribed);

            var filteredSubscribers = new List<Subscriber>
            {
                new Subscriber(){Email = "abc@example.com", SubCategory = "Apps", IsSubscribed = true},
                new Subscriber(){Email = "def@example.com", SubCategory = "Apps", IsSubscribed = true},
                new Subscriber(){Email = "ghi@example.com", SubCategory = "Books", IsSubscribed = true},
                new Subscriber(){Email = "jkl@example.com", SubCategory = "Books", IsSubscribed = false}
            }.Where(s => s.SubCategory == "Apps" && s.IsSubscribed);

            const int Total = 2;

            _repository.Setup(r => r.GetAllEntitiesBySubCategory(It.IsAny<string>()))
                .ReturnsAsync(allEntities);

            _mapper.Setup(m => m.AllEntitiesToModels(allEntities))
                .Returns(filteredSubscribers);

            var result = await _sut.GetAllActiveSubscribersBySubCategory("Apps");

            Assert.Equal(Total, result.Count());
        }

        #endregion

        #region GetItem
        [Fact, Trait("Category","GetItem")]
        public async Task GetItem_ShouldReturnRightResponse_IfArgumentIsNullOrEmpty()
        {
            const string Message = "Please provide the email!";

            var result = await _sut.GetItem("");

            Assert.Equal(Message, result.Message);
        }

        [Fact, Trait("Category", "GetItem")]
        public async Task GetItem_ShouldReturnRightResponse_IfSubscriberIsNull()
        {
            const string Message = "Subscriber with Email abc@example.com not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            var result = await _sut.GetItem("abc@example.com");

            Assert.Equal(Message, result.Message);
        }

        [Fact, Trait("Category", "GetItem")]
        public async Task GetItem_ShouldReturnSubscriber_IfItExists()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>())).Returns(subscribers[0]);

            var response = await _sut.GetItem("abc@example.com");

            var model = response.Data as Subscriber;

            Assert.Equal("abc@example.com", model.Email);
        }

        [Fact, Trait("Category", "GetItem")]
        public async Task GetItems_ShouldCallTheRepoOnce()
        {
            var result = await _sut.GetItem("abc@example.com");

            _repository.Verify(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
        #endregion

        #region AddItem
        [Fact,Trait("Category","AddItem")]
        public async Task AddItem_ShouldReturnResponse_IfEmailAlreadyExists()
        {
            const string Message = "Subscriber with email abc@example.com already exists!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>())).Returns(subscribers[0]);

            var response = await _sut.AddItem(subscribers[0]);

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "AddItem")]
        public async Task AddItem_ShouldCallMapperAndRepo_IfSubscriberValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>())).Returns(() => null);

            var response = await _sut.AddItem(new Subscriber());

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<Subscriber>()), Times.Once());

            _repository.Verify(r => r.AddEntity(It.IsAny<SubscriberEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "AddItem")]
        public async Task AddItem_ShouldReturnRightResponseMessage_IfSubscriberIsAdded()
        {
            const string Message = "Subscriber added successfully!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>())).Returns(() => null);

            _mapper.Setup(m => m.ModelToEntity(It.IsAny<Subscriber>())).Returns(() => null);

            _repository.Setup(r => r.AddEntity(It.IsAny<SubscriberEntity>())).ReturnsAsync(true);

            var response = await _sut.AddItem(new Subscriber());

            Assert.Equal(Message, response.Message);
        }
        #endregion

        #region UpdateItem
        [Fact,Trait("Category","UpdateItem")]
        public async Task UpdateItem_ShouldReturnResponse_IfSubscriberDoesNotExist()
        {
            const string Message = "Subscriber xyz@example.com not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>())).Returns(() => null);

            var response = await _sut.UpdateItem("xyz@example.com", new Subscriber { Email = "xyz@example.com" });

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "UpdateItem")]
        public async Task UpdateItem_ShouldCallMapperAndRepo_IfSubscriberValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>()))
                .Returns(new Subscriber {Email = "xyz@example.com", IsSubscribed = true});

            var response = await _sut.UpdateItem("xyz@example.com", new Subscriber { Email = "xyz@example.com", IsSubscribed = true });

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<Subscriber>()), Times.Once());

            _repository.Verify(r => r.UpdateEntity(It.IsAny<SubscriberEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "UpdateItem")]
        public async Task UpdateItem_ShouldReturnRightResponseMessage_IfSubscriberIsUpdated()
        {
            const string Message = "Subscriber updated successfully!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>()))
               .Returns(new Subscriber { Email = "xyz@example.com", IsSubscribed = true });

            _mapper.Setup(m => m.ModelToEntity(It.IsAny<Subscriber>()))
                .Returns(entities[0]);

            _repository.Setup(r => r.UpdateEntity(It.IsAny<SubscriberEntity>())).ReturnsAsync(true);

            var response = await _sut.UpdateItem("xyz@example.com", new Subscriber { Email = "xyz@example.com", IsSubscribed = true });

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "UpdateItem")]
        public async Task UpdateItem_ShouldReturnRightResponseMessage_IfSubscriberHasBeenUnsubscribed()
        {
            const string Message = "xyz@example.com is not a subscriber!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>()))
               .Returns(new Subscriber { Email = "xyz@example.com", IsSubscribed = false });

            var response = await _sut.UpdateItem("xyz@example.com", new Subscriber { Email = "xyz@example.com", IsSubscribed = false });

            Assert.Equal(Message, response.Message);
        }
        #endregion

        #region DisableItem
        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldReturnResponse_IfSubscriberDoesNotExist()
        {
            const string Message = "Subscriber xyz@example.com not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>())).Returns(() => null);

            var response = await _sut.DisableItem("xyz@example.com");

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldReturnResponse_IfSubscriberHasAlreadyBeenUnsubscribed()
        {
            const string Message = "Subscriber xyz@example.com has already been Unsubscribed!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new SubscriberEntity { RowKey = "xyz@example.com", IsSubscribed = false });

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>()))
                .Returns(new Subscriber { Email = "xyz@example.com", IsSubscribed = false });

            var response = await _sut.DisableItem("xyz@example.com");

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldCallMapperAndRepo_IfSubscriberValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new SubscriberEntity { RowKey = "xyz@example.com", IsSubscribed = true });

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>()))
                .Returns(new Subscriber { Email = "xyz@example.com", IsSubscribed = true });

            var response = await _sut.DisableItem("xyz@example.com");

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<Subscriber>()), Times.Once());

            _repository.Verify(r => r.DiableEntity(It.IsAny<SubscriberEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldReturnRightResponseMessage_IfSubscriberIsUnsubscribed()
        {
            const string Message = "Subscriber xyz@example.com successfully unsubscribed!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new SubscriberEntity { RowKey = "xyz@example.com", IsSubscribed = true });

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>()))
                .Returns(new Subscriber { Email = "xyz@example.com", IsSubscribed = true });

            _repository.Setup(r => r.DiableEntity(It.IsAny<SubscriberEntity>())).ReturnsAsync(true);

            var response = await _sut.DisableItem("xyz@example.com");

            Assert.Equal(Message, response.Message);
        }
        #endregion

        #region DeleteItem
        [Fact, Trait("Category", "DeleteItem")]
        public async Task DeleteItem_ShouldReturnResponse_IfSubscriberDoesNotExist()
        {
            const string Message = "Subscriber xyz@example.com not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>())).Returns(() => null);

            var response = await _sut.DeleteItem("xyz@example.com");

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "DeleteItem")]
        public async Task DeleteItem_ShouldCallMapperAndRepo_IfSubscriberValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>()))
                .Returns(subscribers[0]);

            var response = await _sut.DeleteItem("abc@example.com");

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<Subscriber>()), Times.Once());

            _repository.Verify(r => r.DeleteEntity(It.IsAny<SubscriberEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "DeleteItem")]
        public async Task DeleteItem_ShouldReturnRightResponseMessage_IfSubscriberIsDeleted()
        {
            const string Message = "Subscriber abc@example.com successfully deleted!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<SubscriberEntity>()))
                .Returns(subscribers[0]);

            _repository.Setup(r => r.DeleteEntity(It.IsAny<SubscriberEntity>())).ReturnsAsync(true);

            var response = await _sut.DeleteItem("abc@example.com");

            Assert.Equal(Message, response.Message);
        }
        #endregion
    }
}