#include<queue>
#include<vector>
#include<iostream>



long int get_row(long int& t, long int& m, long int& n)
{
	return t / n;
}

long int get_column(long int& t, long int& m, long int& n)
{
	if (t < n)
	{
		return t;
	}
	else
		return t % n;
}

using namespace std;

int main()
{
	long int m, n, t, n_1, n_2, i, row, c, j;
	vector<long int> d, r;
	vector<int>  level, visited, set;
	queue<long int> q;

	cin >> m >> n;

	for (i = 0; i < m*n; i++)
	{	
		cin >> t;
		r.push_back(t);
		visited.push_back(0);
		level.push_back(-1);
		set.push_back(0);
		
	}

	for (i = 0; i < m*n; i++)
	{
		cin >> t;
		d.push_back(t);
	}

	t = 0;
	level[t] = 0;
	row = get_row(t, m, n);
	c = get_column(t, m, n);
	visited[t] = 1;
	
	if (c + r[t] < n)
	{
		for (i = 1; i <= r[t]; i++)
		{
			if (visited[t + i] == 0 && set[t + i] == 0)
			{
				q.push(t + i);
				level[t + i] = level[t] + 1;
				set[t + i] = 1;
			}
		}


	}
	else
	{
		i = c + 1;
		j = 1;
		while (i<n)
		{
			if (visited[t + j] == 0 && set[t + j] == 0)
			{
				q.push(t + j);
				level[t + j] = level[t] + 1;
				set[t + j] = 1;
			}


			i++;
			j++;
		}
	}

	if (row + d[t] < m)
	{
		for (i = 1; i <= d[t]; i++)
		{
			if (visited[(row + i)*n + c] == 0 && set[(row + i)*n + c] == 0)
			{
				q.push((row + i)*n + c);
				level[(row + i)*n + c] = level[t] + 1;
				set[(row + i)*n + c] = 1;
			}
		}
	}
	else
	{
		i = row + 1;
		j = 1;
		while (i<m)
		{
			if (visited[(row + j)*n + c] == 0 && set[(row + j)*n + c] == 0)
			{
				q.push((row + j)*n + c);
				level[(row + j)*n + c] = level[t] + 1;
				set[(row + j)*n + c] = 1;
			}
			i++;
			j++;
		}
	}


	if (n*m == 1)
	{
		cout << "0";
		return 0;
	}

	while (!q.empty())


	{

		
		t = q.front();
		
		if (t == (n*m - 1))
		{
			cout << level[t];
			
			return 0;
		}

		if (visited[t] == 0)
		{
			
			row = get_row(t, m, n);
			c = get_column(t, m, n);
			visited[t] = 1;

			if (c + r[t] < n)
			{
				for (i = 1; i <= r[t]; i++)
				{
					if (visited[t + i] == 0 && set[t+i] == 0) 
					{
						q.push(t + i);
						level[t + i] = level[t] + 1;
						set[t + i] = 1;
					}
				}
			}
			else
			{
				i = c + 1;
				j = 1;
				while (i<n)
				{	
					if (visited[t + j] == 0 && set[t+j] == 0)
					{
						q.push(t + j);
						level[t + j] = level[t] + 1;
						set[t + j] = 1;
					}

					
					i++;
					j++;
				}
			}

			if (row + d[t] < m)
			{
				for (i = 1; i <= d[t]; i++)
				{
					if (visited[(row + i)*n + c] == 0  && set[(row + i)*n + c]==0)
					{
						q.push((row + i)*n + c);
						level[(row + i)*n + c] = level[t] + 1;
						set[(row + i)*n + c] = 1;
					}
				}
			}
			else
			{
				i = row + 1;
				j = 1;
				while (i<m)
				{
					if (visited[(row + j)*n + c] == 0 && set[(row + j)*n + c] == 0)
					{
						q.push((row + j)*n + c);
						level[(row + j)*n + c] = level[t] + 1;
						set[(row + j)*n + c] = 1;
					}
					i++;
					j++;
				}
			}

			
		}

		q.pop();
	}

	
	return 0;
}
