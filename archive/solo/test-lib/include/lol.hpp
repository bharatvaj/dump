#include <em/EventManager.hpp>

enum class AEvent {
  START,
  EXIT
};

class EventClass : public em::EventManager<AEvent> {
public:
  EventClass();
};
