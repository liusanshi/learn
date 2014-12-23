# coding=utf8

def getN(n,c):
	"""
	获取n个数中 1-9的个数，对于0无效
	"""
	if n< 1:return 0
	length = len(str(n)) - 1
	ic = int(c)
	f = n // (10 ** length)
	m = n % (10 ** length)
	count = f * length * (10 ** (length - 1))
	if f > ic:
		count += 10 ** length
	elif f == ic:
		count += 1 + m
	return count + getN(m, c)

def nSm(n, m):
	"""
	获取 0 的个数：比如 nSm(5,3) 是获取 1-5000 的0的个数
	"""
	if m <= 0: return 0
	i = 2
	count = 1
	if m > 1:
		while i <= m:
			count += 9 * (i - 1) * (10 ** (i - 2)) + 1
			i += 1
	return count + (n - 1)*m*(10**(m-1))

def Sm(m):
	"""
	求 n个数中的0的个数 比如：0001 - 1000 中0 的个数
	"""
	count = 0
	for i in range(1, m):
		count += (m - i) * 9 * (10 ** (i - 1))
	return count + nSm(1, m - 1)