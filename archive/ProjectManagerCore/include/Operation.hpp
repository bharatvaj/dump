#ifndef _PM_OPERATION_H
#define _PM_OPERATION_H

#include <PMEvent.hpp>
#include <em/EventManager.hpp>
#include <model/Project.hpp>
#include <vector>

namespace pm {
class Operation : public em::EventManager<PMEvent, Project *> {

public:
  typedef std::function<void(Operation &)> OperationCallback;

private:
  OperationCallback operationCallback;

public:
  Operation();
  Operation(OperationCallback);
  ~Operation();
};
} // namespace pm

#endif
