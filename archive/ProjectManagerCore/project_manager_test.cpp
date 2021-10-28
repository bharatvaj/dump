#include <iostream>
#include <map>
#include <model/Project.hpp>

#include <ProjectManagerCore.hpp>
#include <csignal>
#include <fmt/format.h>

using namespace pm;
using namespace std;

void exit_handler(int sig) { exit(EXIT_SUCCESS); }

int main(int argc, char *argv[]) {
  signal(SIGINT, exit_handler);

  std::vector<Project *> v;

  ProjectManagerCore pmc;
  Project p("SOLO", "IoT Automation Project", "https://github.com/solo/solo",
            "git");
  pmc.addProject(p).on(PMEvent::SUCCESS, [](Project *p) {
    fmt::print("Added {}\n", p->name);
  });

  pmc.getProjects(Query())
      .on(PMEvent::FATAL_ERROR,
          [](Project *p) { std::cout << "Fatal error" << std::endl; })
      .on(PMEvent::NOT_FOUND,
          [](Project *p) { std::cout << "Not found" << std::endl; })
      .on(PMEvent::SUCCESS, [&](Project *p) {
        v.push_back(p);
      });

  for(auto p : v){
    pmc.removeProject(*p).on(PMEvent::SUCCESS, [](Project *p){
      fmt::print("Removed {}\n", p->name);
    });
  }

  // pmc.modifyProject(p).on(PMEvent::SUCCESS, [](Project *p) {
  //   std::cout << "modified" << std::endl;
  // });
  // pmc.removeProject(p).on(PMEvent::SUCCESS, [](Project *p) {
  //   std::cout << "removed" << std::endl;
  // });

  while (1) {
    //
  }
  return 0;
}
