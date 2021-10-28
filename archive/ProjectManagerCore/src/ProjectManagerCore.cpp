#include <ProjectManagerCore.hpp>

#include <map>

#include <Operation.hpp>
#include <fmt/format.h>
#include <model/Project.hpp>
#include <sqlite3.h>
#include <thread>
#include <unistd.h>

using namespace pm;

pm::ProjectManagerCore::ProjectManagerCore() {
  std::string query =
      "create table pmc_table(uid text, name text, description "
      "text, repositoryType text, repositoryLink text, primary key (uid))";
  sqlite3_stmt **stmt = (sqlite3_stmt **)malloc(sizeof(sqlite3_stmt *));
  db = (sqlite3 **)malloc(sizeof(sqlite3 *));
  if (sqlite3_open("local.db", db) == SQLITE_OK) {
    sqlite3_exec(*db, query.c_str(), nullptr, nullptr, nullptr);
  } else
    delete this;
}

static int s_callback(void *op, int argc, char **argv, char **col) {
  Project *p = new Project(argv[0], argv[1], argv[2], argv[3], argv[4]);
  ((Operation *)op)->fireEvent(PMEvent::SUCCESS, p);
  return 0;
}

pm::Operation pm::ProjectManagerCore::getProjects(Query query) {
  // TODO get the project based on the query
  return Operation([&](Operation &op) {
    std::string getQuery = fmt::format("select * from pmc_table");
    sqlite3_exec(*db, getQuery.c_str(), s_callback, &op, nullptr);
    // TODO if empty fire not found and return
    // TODO else return the results
  });
}

pm::Operation pm::ProjectManagerCore::addProject(pm::Project p) {
  return Operation([&](Operation &op) {
    // assuming there is no error with map
    // error is checked only for persistence storage
    std::string query = fmt::format(
        "insert into pmc_table values('{}', '{}', '{}', '{}', '{}')",
        p.getUid(), p.name, p.description, p.repositoryLink, p.repositoryType);
    sqlite3_exec(*db, query.c_str(), nullptr, nullptr, nullptr);
    op.fireEvent(PMEvent::SUCCESS, new Project(p));
  });
}

pm::Operation pm::ProjectManagerCore::modifyProject(pm::Project &p) {
  return Operation([&](Operation &op) {
    if (false) {
      // op.fireEvent(PMEvent::NOT_FOUND, v);
    } else {
      std::string query =
          fmt::format("update pmc_table set name='{}', description='{}', "
                      "repositoryLink='{}', repositoryType='{}' where "
                      "uid='{}'",
                      p.name, p.description, p.repositoryLink, p.repositoryType,
                      p.getUid());
      sqlite3_exec(*db, query.c_str(), nullptr, nullptr, nullptr);
      op.fireEvent(PMEvent::SUCCESS, &p);
      // error is checked only for persistence storage
    }
  });
}

pm::Operation pm::ProjectManagerCore::removeProject(pm::Project &p) {
  return Operation([&](Operation &op) {
    std::string query =
        fmt::format("delete from pmc_table where uid='{}'", p.getUid());
        sqlite3_exec(*db, query.c_str(),nullptr, nullptr, nullptr);
        op.fireEvent(PMEvent::SUCCESS, &p);
  });
}

pm::ProjectManagerCore::~ProjectManagerCore() {
  if (db != nullptr) {
    sqlite3_close(*db);
  }
}
