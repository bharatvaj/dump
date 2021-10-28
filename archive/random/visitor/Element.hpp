#include "Visitor.hpp"

class Element {
public:
  void accept(Visitor visitor);
};
