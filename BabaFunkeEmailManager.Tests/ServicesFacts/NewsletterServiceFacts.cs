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
    public class NewsletterServiceFacts
    {
        private readonly NewsletterService _sut;
        private readonly Mock<IRepository<NewsletterEntity>> _repository;
        private readonly Mock<IMapper<Newsletter, NewsletterEntity>> _mapper;
        private readonly List<NewsletterEntity> entities;
        private readonly List<Newsletter> newsletters;

        public NewsletterServiceFacts()
        {
            _repository = new Mock<IRepository<NewsletterEntity>>();
            _mapper = new Mock<IMapper<Newsletter, NewsletterEntity>>();
            _sut = new NewsletterService(_repository.Object, _mapper.Object);

            entities = new List<NewsletterEntity>
            {
                new NewsletterEntity(){RowKey = "Issue_1", PartitionKey = "Newsletter"},
                new NewsletterEntity(){RowKey = "Issue_2", PartitionKey = "Newsletter"}
            };

            newsletters = new List<Newsletter>
            {
                new Newsletter(){NewsletterId = "Issue_1"},
                new Newsletter(){NewsletterId = "Issue_2"}
            };
        }

        #region GetAllItems
        [Fact, Trait("Category", "GetAllItems")]
        public async Task GetAllItems_ShouldReturnTheRightTotal_WhenNotEmpty()
        {
            const int Total = 2;

            _repository.Setup(r => r.GetAllEntities()).ReturnsAsync(entities);

            _mapper.Setup(m => m.AllEntitiesToModels(entities)).Returns(newsletters);

            var result = await _sut.GetAllItems();

            Assert.Equal(Total, result.Count());
        }

        [Fact, Trait("Category", "GetAllItems")]
        public async Task GetAllItems_ShouldReturnEmptyList_WhenEmpty()
        {
            _repository.Setup(r => r.GetAllEntities()).ReturnsAsync(new List<NewsletterEntity>());

            _mapper.Setup(m => m.AllEntitiesToModels(It.IsAny<List<NewsletterEntity>>())).Returns(new List<Newsletter>());

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
            const string Message = "Newsletter Id cannot be null or empty!";

            var result = await _sut.GetItem("");

            Assert.Equal(Message, result.Message);
        }

        [Fact, Trait("Category", "GetItem")]
        public async Task GetItem_ShouldReturnRightResponse_IfNewsletterIsNull()
        {
            const string Message = "Newsletter with Id Issue_3 not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            var result = await _sut.GetItem("Issue_3");

            Assert.Equal(Message, result.Message);
        }

        [Fact, Trait("Category", "GetItem")]
        public async Task GetItem_ShouldReturnNewsletter_IfItExists()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>())).Returns(newsletters[0]);

            var response = await _sut.GetItem("Issue_1");

            var model = response.Data;

            Assert.Equal("Issue_1", model.NewsletterId);
        }

        [Fact, Trait("Category", "GetItem")]
        public async Task GetItems_ShouldCallTheRepoOnce()
        {
            var result = await _sut.GetItem("Issue_1");

            _repository.Verify(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
        #endregion

        #region AddItem
        [Fact,Trait("Category","AddItem")]
        public async Task AddItem_ShouldReturnResponse_IfNewsletterIdAlreadyExists()
        {
            const string Message = "Newsletter with Id Issue_1 already exists!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>())).Returns(newsletters[0]);

            var response = await _sut.AddItem(newsletters[0]);

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "AddItem")]
        public async Task AddItem_ShouldCallMapperAndRepo_IfNewsletterValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>())).Returns(() => null);

            var response = await _sut.AddItem(new Newsletter());

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<Newsletter>()), Times.Once());

            _repository.Verify(r => r.AddEntity(It.IsAny<NewsletterEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "AddItem")]
        public async Task AddItem_ShouldReturnRightResponseMessage_IfNewsletterIsAdded()
        {
            const string Message = "Newsletter added successfully!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>())).Returns(() => null);

            _mapper.Setup(m => m.ModelToEntity(It.IsAny<Newsletter>())).Returns(() => null);

            _repository.Setup(r => r.AddEntity(It.IsAny<NewsletterEntity>())).ReturnsAsync(true);

            var response = await _sut.AddItem(new Newsletter());

            Assert.Equal(Message, response.Message);
        }
        #endregion

        #region UpdateItem
        [Fact,Trait("Category","UpdateItem")]
        public async Task UpdateItem_ShouldReturnResponse_IfNewsletterDoesNotExist()
        {
            const string Message = "Newsletter with Id Issue_3 not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>())).Returns(() => null);

            var response = await _sut.UpdateItem("Issue_3", new Newsletter { NewsletterId = "Issue_3" });

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "UpdateItem")]
        public async Task UpdateItem_ShouldCallMapperAndRepo_IfNewsletterValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>()))
                .Returns(new Newsletter { NewsletterId = "Issue_1", Subject = "Hello", Body = "Greetings..." });

            var response = await _sut.UpdateItem("Issue_1", new Newsletter { NewsletterId = "Issue_1", Subject = "Hello", Body = "Greetings..." });

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<Newsletter>()), Times.Once());

            _repository.Verify(r => r.UpdateEntity(It.IsAny<NewsletterEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "UpdateItem")]
        public async Task UpdateItem_ShouldReturnRightResponseMessage_IfNewsletterIsUpdated()
        {
            const string Message = "Newsletter updated successfully!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>()))
               .Returns(new Newsletter { NewsletterId = "Issue_1", Subject = "Hello", Body = "Greetings..." });

            _mapper.Setup(m => m.ModelToEntity(It.IsAny<Newsletter>()))
                .Returns(entities[0]);

            _repository.Setup(r => r.UpdateEntity(It.IsAny<NewsletterEntity>())).ReturnsAsync(true);

            var response = await _sut.UpdateItem("Issue_1", new Newsletter { NewsletterId = "Issue_1", Subject = "Hello", Body = "Greetings..." });

            Assert.Equal(Message, response.Message);
        }
        #endregion

        #region DisableItem
        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldReturnResponse_IfNewsletterDoesNotExist()
        {
            const string Message = "Newsletter with Id Issue_3 not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>())).Returns(() => null);

            var response = await _sut.DisableItem("Issue_3");

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldCallMapperAndRepo_IfNewsletterValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new NewsletterEntity { RowKey = "Issue_1" });

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>()))
                .Returns(new Newsletter { NewsletterId = "Issue_1"});

            var response = await _sut.DisableItem("Issue_1");

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<Newsletter>()), Times.Once());

            _repository.Verify(r => r.DiableEntity(It.IsAny<NewsletterEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "DisableItem")]
        public async Task DisableItem_ShouldReturnRightResponseMessage_IfNewsletterIsDisabled()
        {
            const string Message = "Newsletter Issue_1 successfully disabled!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new NewsletterEntity { RowKey = "Issue_1" });

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>()))
                .Returns(new Newsletter { NewsletterId = "Issue_1" });

            _repository.Setup(r => r.DiableEntity(It.IsAny<NewsletterEntity>())).ReturnsAsync(true);

            var response = await _sut.DisableItem("Issue_1");

            Assert.Equal(Message, response.Message);
        }
        #endregion

        #region DeleteItem
        [Fact, Trait("Category", "DeleteItem")]
        public async Task DeleteItem_ShouldReturnResponse_IfNewsletterDoesNotExist()
        {
            const string Message = "Newsletter with Id Issue_3 not found!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>())).Returns(() => null);

            var response = await _sut.DeleteItem("Issue_3");

            Assert.Equal(Message, response.Message);
        }

        [Fact, Trait("Category", "DeleteItem")]
        public async Task DeleteItem_ShouldCallMapperAndRepo_IfNewsletterValid()
        {
            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>()))
                .Returns(newsletters[0]);

            var response = await _sut.DeleteItem("Issue_1");

            _mapper.Verify(m => m.ModelToEntity(It.IsAny<Newsletter>()), Times.Once());

            _repository.Verify(r => r.DeleteEntity(It.IsAny<NewsletterEntity>()), Times.Once());
        }

        [Fact, Trait("Category", "DeleteItem")]
        public async Task DeleteItem_ShouldReturnRightResponseMessage_IfNewsletterIsDeleted()
        {
            const string Message = "Newsletter Issue_1 successfully deleted!";

            _repository.Setup(r => r.GetEntity(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entities[0]);

            _mapper.Setup(m => m.EntityToModel(It.IsAny<NewsletterEntity>()))
                .Returns(newsletters[0]);

            _repository.Setup(r => r.DeleteEntity(It.IsAny<NewsletterEntity>())).ReturnsAsync(true);

            var response = await _sut.DeleteItem("Issue_1");

            Assert.Equal(Message, response.Message);
        }
        #endregion
    }
}
