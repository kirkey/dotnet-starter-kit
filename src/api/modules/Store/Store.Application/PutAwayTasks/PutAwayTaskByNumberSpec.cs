namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks;

public sealed class PutAwayTaskByNumberSpec : Specification<PutAwayTask>
{
    public PutAwayTaskByNumberSpec(string taskNumber)
    {
        Query.Where(x => x.TaskNumber == taskNumber);
    }
}
