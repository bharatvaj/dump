#ifndef _POS_H
#define _POS_H
class Pos
{
  private:
    int x;
    int y;

  public:
    Pos()
    {
    }
    Pos(int x, int y)
    {
        this->x = x;
        this->y = y;
    }

    void setX(int x)
    {
        this->x = x;
    }

    void setY(int y)
    {
        this->y = y;
    }

    int getX()
    {
        return x;
    }

    int getY()
    {
        return y;
    }
};
#endif