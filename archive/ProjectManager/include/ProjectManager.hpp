#ifndef _PM_H
#define _PM_H

#include <PMAEvent.hpp>
#include <em/EventManager.hpp>
#include <model/Project.hpp>

namespace pm {
class ProjectManager : public em::EventManager<PMAEvent, pm::Project *> {
public:
  ProjectManager();
};
} // namespace pm

#endif
