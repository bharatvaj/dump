#ifndef _PROJECT_MAN_CORE_H
#define _PROJECT_MAN_CORE_H

#include <Operation.hpp>
#include <Query.hpp>
#include <map>
#include <model/Project.hpp>

#include <sqlite3.h>

namespace pm {
class ProjectManagerCore {
private:
  sqlite3 **db = nullptr;

public:
  // initializes the ProjectManagerCore, reads local files and adds to memory
  ProjectManagerCore();
  // Returns an active object, WARNING: do not delete the returned object
  Operation getProjects(Query);
  Operation addProject(Project);
  Operation modifyProject(Project &);
  Operation removeProject(Project &);

  ~ProjectManagerCore();
};
} // namespace pm

#endif
