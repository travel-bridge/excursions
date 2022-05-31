namespace Excursions.Application.IntegrationEvents;

public interface IIntegrationEvent
{
    string GetTopic();
}