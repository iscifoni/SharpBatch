using System;
using SharpBatch.Serialization.Xml;
using Xunit;

namespace SharpBatch.Serialization.XmlTest
{
    public class ModelXmlSerializerTest
    {
        [Fact]
        public void ModelXmlSerializer_Serialize()
        {
            // Arrange
            var modelSerializer = new ModelXmlSerializer();
            var modelToSerialize = new ModelToSerialize()
            {
                Name = "Name",
                Surname = "Surname"
            };

            var expectedSerialization = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ModelToSerialize xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Name>Name</Name><Surname>Surname</Surname></ModelToSerialize>";
            //Act
            var response = modelSerializer.Serialize(modelToSerialize);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(expectedSerialization, response);
        }

        [Fact]
        public void ModelXmlSerializer_SerializeOf()
        {
            // Arrange
            var modelSerializer = new ModelXmlSerializer();
            var modelToSerialize = new ModelToSerialize()
            {
                Name = "Name",
                Surname = "Surname"
            };

            var expectedSerialization = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ModelToSerialize xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Name>Name</Name><Surname>Surname</Surname></ModelToSerialize>";
            //Act
            var response = modelSerializer.Serialize<ModelToSerialize>(modelToSerialize);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(expectedSerialization, response);
        }


        [Fact]
        public void ModelXmlSerializer_Deserialize()
        {
            // Arrange
            var modelSerializer = new ModelXmlSerializer();
            var contentToSerialize = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ModelToSerialize xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Name>Name</Name><Surname>Surname</Surname></ModelToSerialize>";
            var modelExpected = new ModelToSerialize()
            {
                Name = "Name",
                Surname = "Surname"
            };

            //Act
            var response = modelSerializer.Deserialize<ModelToSerialize>(contentToSerialize);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(modelExpected.Name, response.Name);
            Assert.Equal(modelExpected.Surname, response.Surname);

        }

        [Serializable]
        public class ModelToSerialize
        {
            public string Name { get; set; }
            public string Surname { get; set; }

        }
    }
}
