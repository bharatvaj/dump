#ifndef _PROJECT_MODEL_H
#define _PROJECT_MODEL_H

#include <crossguid/guid.hpp>
#include <iostream>

namespace pm {
class Project {
private:
  std::string uid;

public:
  std::string name;
  std::string description;
  std::string repositoryType;
  std::string repositoryLink;

  std::string getUid() { return uid; }

  Project() { uid = xg::newGuid().str(); }
  Project(std::string uid, std::string name, std::string description,
          std::string repositoryLink, std::string repositoryType) {
    this->uid = uid;
    this->name = name;
    this->description = description;
    this->repositoryType = repositoryType;
    this->repositoryLink = repositoryLink;
  }
  Project(std::string name, std::string description, std::string repositoryLink,
          std::string repositoryType)
      : Project(xg::newGuid().str(), name, description, repositoryLink,
                repositoryType) {}
  Project(const Project &p)
      : Project(p.uid, p.name, p.description, p.repositoryLink,
                p.repositoryType) {}
};
} // namespace pm

#endif
