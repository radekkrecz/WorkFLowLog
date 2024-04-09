using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.ApplicationServices.Services;

public interface IProjectsProvider
{
    void InsertProjects();

    void AddProject();

    void EditProject();

    void RemoveProject();

    void ReadAllProjects();

    Project LookForProjectByName(string name);

    Project GetProjectByName(string name);

    Project? GetProjectById(int id);
}
