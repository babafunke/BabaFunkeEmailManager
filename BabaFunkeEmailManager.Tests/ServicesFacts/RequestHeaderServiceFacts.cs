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
    public class RequestHeaderServiceFacts
    {
        private readonly RequestHeaderService _sut;
        private readonly Mock<IRepository<RequestHeaderEntity>> _repository;
        private readonly Mock<IMapper<RequestHeader, RequestHeaderEntity>> _mapper;
        private readonly List<RequestHeaderEntity> entities;
        private readonly List<RequestHeader> RequestHeaders;

        public RequestHeaderServiceFacts()
        {
            _repository = new Mock<IRepository<RequestHeaderEntity>>();
            _mapper = new Mock<IMapper<RequestHeader, RequestHeaderEntity>>();
            _sut = new RequestHeaderService(_repository.Object, _mapper.Object);

            entities = new List<RequestHeaderEntity>
            {
                new RequestHeaderEntity(){RowKey = "1", PartitionKey = "Subscribers"},
                new RequestHeaderEntity(){RowKey = "2", PartitionKey = "Subscribers"}
            };

            RequestHeaders = new List<RequestHeader>
            {
                new RequestHeader(){RequestHeaderId = "1"},
                new RequestHeader(){RequestHeaderId = "2"}
            };
        }

        #region GetAllItems
        [Fact, Trait("Category", "GetAllItems")]
        public async Task GetAllItems_ShouldReturnTheRightTotal_WhenNotEmpty()
        {
            const int Total = 2;

            _repository.Setup(r => r.GetAllEntities()).ReturnsAsync(entities);

            _mapper.Setup(m => m.AllEntitiesToModels(entities)).Returns(RequestHeaders);

            var result = await _sut.GetAllItems();

            Assert.Equal(Total, result.Count());
        }

        [Fact, Trait("Category", "GetAllItems")]
        public async Task GetAllItems_ShouldReturnEmptyList_WhenEmpty()
        {
            _repository.Setup(r => r.GetAllEntities()).ReturnsAsync(new List<RequestHeaderEntity>());

            _mapper.Setup(m => m.AllEntitiesToModels(It.IsAny<List<RequestHeaderEntity>>())).Returns(new List<RequestHeader>());

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

        #region GetItem
        [Fact, Trait("Category","GetItem")]
        public async Task GetItem_ShouldReturnRightResponse_IfArgumentIsNullOrEmpty()
        {
            const string Message = "RequestHeader Id cannot be null or empty!";

            var result = await _sut.GetItem("");

            Assert.Equal(Message, result.Message);
        }

        [Fact, Trait("Category", "GetItem")]
        public async Task GetItem_ShouldReturnRightResponse_IfRequestHeaderIsNull()
        {
            const string Message = "RequestHeader with Id 3 not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            var result = await _sut.GetItem("3");

            Assert.Equal(Message, result.Message);
        }

        [Fact, Trait("Category", "GetItem")]
        public async Task GetItem_ShouldReturnRequestHeader_IfItExists()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>())).Returns(RequestHeaders[0]);

            var response = await _sut.GetItem("1");

            var model = response.Data;

            Assert.Equal("1", model.RequestHeaderId);
        }

        [Fact, Trait("Category", "GetItem")]
        public async Task GetItems_ShouldCallTheRepoOnce()
        {
            var result = await _sut.GetItem("1");

            _repository.Verify(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
        #endregion

        #region AddItem
        
        [Fact, Trait("Category", "AddItem")]
        public async Task AddItem_ShouldCallMapperAndRepo_IfRequestHeaderValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>())).Returns(() => null);

            var response = await _sut.AddItem(new RequestHeader());

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<RequestHeader>()), Times.Once());

            _repository.Verify(r => r.AddEntity(It.IsAny<RequestHeaderEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "AddItem")]
        public async Task AddItem_ShouldReturnRightResponseMessage_IfRequestHeaderIsAdded()
        {
            const string Message = "RequestHeader added successfully!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>())).Returns(() => null);

            _mapper.Setup(m => m.ModelToEntity(It.IsAny<RequestHeader>())).Returns(() => null);

            _repository.Setup(r => r.AddEntity(It.IsAny<RequestHeaderEntity>())).ReturnsAsync(true);

            var response = await _sut.AddItem(new RequestHeader());

            Assert.Equal(Message, response.Message);
        }
        #endregion

        #region UpdateItem
        [Fact,Trait("Category","UpdateItem")]
        public async Task UpdateItem_ShouldReturnResponse_IfRequestHeaderDoesNotExist()
        {
            const string Message = "RequestHeader with Id 3 not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>())).Returns(() => null);

            var response = await _sut.UpdateItem("3", new RequestHeader { RequestHeaderId = "3" });

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "UpdateItem")]
        public async Task UpdateItem_ShouldCallMapperAndRepo_IfRequestHeaderValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>()))
                .Returns(new RequestHeader { RequestHeaderId = "1", NewsletterId = "Issue_1", SubscriberSubCategory = "Subscribers_2" });

            var response = await _sut.UpdateItem("1", new RequestHeader { RequestHeaderId = "1", NewsletterId = "Issue_1", SubscriberSubCategory = "Subscribers_2" });

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<RequestHeader>()), Times.Once());

            _repository.Verify(r => r.UpdateEntity(It.IsAny<RequestHeaderEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "UpdateItem")]
        public async Task UpdateItem_ShouldReturnRightResponseMessage_IfRequestHeaderIsUpdated()
        {
            const string Message = "RequestHeader updated successfully!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>()))
               .Returns(new RequestHeader { RequestHeaderId = "1", NewsletterId = "Issue_1", SubscriberSubCategory = "Subscribers_2" });

            _mapper.Setup(m => m.ModelToEntity(It.IsAny<RequestHeader>()))
                .Returns(entities[0]);

            _repository.Setup(r => r.UpdateEntity(It.IsAny<RequestHeaderEntity>())).ReturnsAsync(true);

            var response = await _sut.UpdateItem("1", new RequestHeader { RequestHeaderId = "1", NewsletterId = "Issue_1", SubscriberSubCategory = "Subscribers_2" });

            Assert.Equal(Message, response.Message);
        }
        #endregion

        #region DisableItem
        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldReturnResponse_IfRequestHeaderDoesNotExist()
        {
            const string Message = "RequestHeader with Id 3 not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>())).Returns(() => null);

            var response = await _sut.DisableItem("3");

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldCallMapperAndRepo_IfRequestHeaderValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new RequestHeaderEntity { RowKey = "1" });

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>()))
                .Returns(new RequestHeader { RequestHeaderId = "1"});

            var response = await _sut.DisableItem("1");

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<RequestHeader>()), Times.Once());

            _repository.Verify(r => r.DiableEntity(It.IsAny<RequestHeaderEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldReturnRightResponseMessage_IfRequestHeaderIsDisabled()
        {
            const string Message = "RequestHeader 1 successfully disabled!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new RequestHeaderEntity { RowKey = "1" });

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>()))
                .Returns(new RequestHeader { RequestHeaderId = "1" });

            _repository.Setup(r => r.DiableEntity(It.IsAny<RequestHeaderEntity>())).ReturnsAsync(true);

            var response = await _sut.DisableItem("1");

            Assert.Equal(Message, response.Message);
        }
        #endregion

        #region DeleteItem
        [Fact, Trait("Category", "DeleteItem")]
        public async Task DeleteItem_ShouldReturnResponse_IfRequestHeaderDoesNotExist()
        {
            const string Message = "RequestHeader with Id 3 not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>())).Returns(() => null);

            var response = await _sut.DeleteItem("3");

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "DeleteItem")]
        public async Task DeleteItem_ShouldCallMapperAndRepo_IfRequestHeaderValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>()))
                .Returns(RequestHeaders[0]);

            var response = await _sut.DeleteItem("1");

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<RequestHeader>()), Times.Once());

            _repository.Verify(r => r.DeleteEntity(It.IsAny<RequestHeaderEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "DeleteItem")]
        public async Task DeleteItem_ShouldReturnRightResponseMessage_IfRequestHeaderIsDeleted()
        {
            const string Message = "RequestHeader 1 successfully deleted!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<RequestHeaderEntity>()))
                .Returns(RequestHeaders[0]);

            _repository.Setup(r => r.DeleteEntity(It.IsAny<RequestHeaderEntity>())).ReturnsAsync(true);

            var response = await _sut.DeleteItem("1");

            Assert.Equal(Message, response.Message);
        }
        #endregion
    }
}
