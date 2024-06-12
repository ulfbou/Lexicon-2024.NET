using Moq;
using MachineryFleet.Core.Entities;
using MachineryFleet.Core.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MachineryFleet.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using MachineryFleet.Tests.Helpers;
using MachineryFleet.Core.Services;

namespace MachineryFleet.Tests
{
    [TestClass]
    public class MachineServiceTests
    {
        private Mock<IMachineRepository> _mockMachineRepository;
        private IMachineService _machineService;

        [TestInitialize]
        public void Setup()
        {
            _mockMachineRepository = new Mock<IMachineRepository>();
            _machineService = new MachineService(_mockMachineRepository.Object);
        }

        [TestMethod]
        public async Task GetLatestLogEntryAsync_ShouldReturnLatestLogEntry_WhenMachineExists()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A", "Data B" } };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetLatestLogEntryAsync(machine.Id);

            // Assert
            Assert.AreEqual("Data B", result);
        }

        [TestMethod]
        public async Task GetLatestLogEntryAsync_ShouldThrowException_WhenMachineDoesNotExist()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _machineService.GetLatestLogEntryAsync(Guid.NewGuid()));
        }

        [TestMethod]
        public async Task GetLatestLogEntryAsync_ShouldThrowException_WhenNoLogEntriesExist()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string>() };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _machineService.GetLatestLogEntryAsync(machine.Id));
        }

        [TestMethod]
        public async Task GetMachineStatusAsync_ShouldReturnMachineStatus_WhenMachineExists()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetMachineStatusAsync(machine.Id);

            // Assert
            Assert.AreEqual(MachineStatus.Inactive, result);
        }

        [TestMethod]
        public async Task GetMachineStatusAsync_ShouldThrowException_WhenMachineDoesNotExist()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _machineService.GetMachineStatusAsync(Guid.NewGuid()));
        }

        [TestMethod]
        public async Task AddLogEntryAsync_ShouldAddLogEntry_WhenMachineExists()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            var entry = EntityEntryHelper.CreateEntityEntry(machine);
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);
            _mockMachineRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Machine>(), It.IsAny<CancellationToken>())).ReturnsAsync(entry);

            // Act
            await _machineService.AddLogEntryAsync(machine.Id, "Data B");

            // Assert
            Assert.AreEqual(2, machine.LogEntries.Count);
            Assert.IsTrue(machine.LogEntries.Last().Contains("Data B"));
        }

        [TestMethod]
        public async Task AddLogEntryAsync_ShouldThrowException_WhenMachineDoesNotExist()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _machineService.AddLogEntryAsync(Guid.NewGuid(), "Data"));
        }

        [TestMethod]
        public async Task AddLogEntryAsync_ShouldThrowException_WhenLogEntryIsNull()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _machineService.AddLogEntryAsync(machine.Id, null));
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllMachines()
        {
            // Arrange
            var machines = new List<Machine>
            {
                new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } },
                new Machine { Id = Guid.NewGuid(), Name = "Machine B", Status = MachineStatus.Active, LogEntries = new List<string> { "Data B" } }
            };
            _mockMachineRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(machines);

            // Act
            var result = await _machineService.GetAllAsync() as List<Machine>;

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Machine A", result[0].Name);
            Assert.AreEqual("Machine B", result[1].Name);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnMachine_WhenMachineExists()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetAsync(machine.Id);

            // Assert
            Assert.AreEqual(machine, result);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnNull_WhenMachineDoesNotExist()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddMachine_WhenMachineIsValid()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            var entry = EntityEntryHelper.CreateEntityEntry(machine);
            _mockMachineRepository.Setup(repo => repo.AddAsync(It.IsAny<Machine>(), It.IsAny<CancellationToken>())).ReturnsAsync(entry);

            // Act
            await _machineService.AddAsync(machine);

            // Assert
            _mockMachineRepository.Verify(repo => repo.AddAsync(machine, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task AddAsync_ShouldThrowException_WhenMachineIsNull()
        {
            // Arrange
            Machine machine = null;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _machineService.AddAsync(machine));
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateMachine_WhenMachineExists()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            var entry = EntityEntryHelper.CreateEntityEntry(machine, EntityState.Modified);
            _mockMachineRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Machine>(), It.IsAny<CancellationToken>())).ReturnsAsync(entry);

            // Act
            await _machineService.UpdateAsync(machine);

            // Assert
            _mockMachineRepository.Verify(repo => repo.UpdateAsync(machine, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldThrowException_WhenMachineDoesNotExist()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Machine>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult((EntityEntry<Machine>?)null));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _machineService.UpdateAsync(machine!));
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldThrowException_WhenMachineIsNull()
        {
            // Arrange
            Machine machine = null;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _machineService.UpdateAsync(machine));
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteMachine_WhenMachineExists()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            var entry = EntityEntryHelper.CreateEntityEntry(machine, EntityState.Deleted);
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);
            _mockMachineRepository.Setup(repo => repo.RemoveAsync(It.IsAny<Machine>(), It.IsAny<CancellationToken>())).ReturnsAsync(entry);

            // Act
            await _machineService.DeleteAsync(machine.Id);

            // Assert
            _mockMachineRepository.Verify(repo => repo.RemoveAsync(machine, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldThrowException_WhenMachineDoesNotExist()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _machineService.DeleteAsync(Guid.NewGuid()));
        }

        [TestMethod]
        public async Task StartAsync_ShouldStartMachine_WhenMachineExists()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            var entry = EntityEntryHelper.CreateEntityEntry(machine, EntityState.Modified);
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);
            _mockMachineRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Machine>(), It.IsAny<CancellationToken>())).ReturnsAsync(entry);

            // Act
            await _machineService.StartAsync(machine.Id);

            // Assert
            Assert.AreEqual(MachineStatus.Active, machine.Status);
            _mockMachineRepository.Verify(repo => repo.UpdateAsync(machine, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task StartAsync_ShouldThrowException_WhenMachineDoesNotExist()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _machineService.StartAsync(Guid.NewGuid()));
        }

        [TestMethod]
        public async Task StopAsync_ShouldStopMachine_WhenMachineExists()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Active, LogEntries = new List<string> { "Data A" } };
            var entry = EntityEntryHelper.CreateEntityEntry(machine, EntityState.Modified);

            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);
            _mockMachineRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Machine>(), It.IsAny<CancellationToken>())).ReturnsAsync(entry);

            // Act
            await _machineService.StopAsync(machine.Id);

            // Assert
            Assert.AreEqual(MachineStatus.Inactive, machine.Status);
            _mockMachineRepository.Verify(repo => repo.UpdateAsync(machine, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task StopAsync_ShouldThrowException_WhenMachineDoesNotExist()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _machineService.StopAsync(Guid.NewGuid()));
        }

        [TestMethod]
        public async Task GetLogEntriesAsync_ShouldReturnLogEntries_WhenMachineExists()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetLogEntriesAsync(machine.Id);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Data A", result.First());
        }

        [TestMethod]
        public async Task GetLogEntriesAsync_ShouldThrowException_WhenMachineDoesNotExist()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _machineService.GetLogEntriesAsync(Guid.NewGuid()));
        }

        /*===================================================================================================*/

        [TestMethod]
        public async Task AddAsync_ShouldAddMachine()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            var entityEntry = EntityEntryHelper.CreateEntityEntry(machine);

            _mockMachineRepository.Setup(repo => repo.AddAsync(It.IsAny<Machine>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(entityEntry);

            // Act
            await _machineService.AddAsync(machine);

            // Assert
            _mockMachineRepository.Verify(repo => repo.AddAsync(machine, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task GetAllMachinesAsync_ShouldReturnAllMachines()
        {
            // Arrange
            var machines = new List<Machine>
            {
                new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } },
                new Machine { Id = Guid.NewGuid(), Name = "Machine B", Status = MachineStatus.Active, LogEntries = new List<string> { "Data B" } }
            };
            _mockMachineRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(machines);

            // Act
            var result = await _machineService.GetAllAsync() as List<Machine>;

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Machine A", result[0].Name);
            Assert.AreEqual("Machine B", result[1].Name);
        }

        // Generate positive test methods for all the public methods in the MachineService class.
        [TestMethod]
        public async Task GetLatestLogEntryAsync_ShouldReturnLatestLogEntry()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A", "Data B" } };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetLatestLogEntryAsync(machine.Id);

            // Assert
            Assert.AreEqual("Data B", result);
        }

        [TestMethod]
        public async Task AddLogEntryAsync_ShouldAddLogEntry()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            var entry = EntityEntryHelper.CreateEntityEntry(machine);
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);
            _mockMachineRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Machine>(), It.IsAny<CancellationToken>())).ReturnsAsync(entry);

            // Act
            await _machineService.AddLogEntryAsync(machine.Id, "Data B");

            // Assert
            Assert.AreEqual(2, machine.LogEntries.Count);
            Assert.IsTrue(machine.LogEntries.Last().Contains("Data B"));
        }

        [TestMethod]
        public async Task GetMachineStatusAsync_ShouldReturnMachineStatus()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetMachineStatusAsync(machine.Id);

            // Assert
            Assert.AreEqual(MachineStatus.Inactive, result);
        }

        [TestMethod]
        public async Task GetLogEntriesAsync_ShouldReturnMachineLogEntries()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetLogEntriesAsync(machine.Id);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Data A", result.First());
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnMachine()
        {
            // Arrange
            var machine = new Machine { Id = Guid.NewGuid(), Name = "Machine A", Status = MachineStatus.Inactive, LogEntries = new List<string> { "Data A" } };
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetAsync(machine.Id);

            // Assert
            Assert.AreEqual(machine, result);
        }

        // Generate negative test methods for all the public methods in the MachineService class to test the functionality when expected items do not exist and vice versa. Ignore testing null values. 
        // For example, ignore a test method to check if the GetLatestLogEntryAsync method throws an exception when the machine is not found.

        [TestMethod]
        public async Task GetAsync_ShouldReturnNull_WhenMachineIsNotFound()
        {
            // Arrange
            Machine machine = null;
            _mockMachineRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(machine);

            // Act
            var result = await _machineService.GetAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }
    }
}
