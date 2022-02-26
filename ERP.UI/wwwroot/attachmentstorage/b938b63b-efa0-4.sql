--SELECT	EmployeeName,MobileNo, 
--		LEFT(MobileNo, 4) + '-' + SUBSTRING(MobileNo, 5, 2) + '-' + SUBSTRING(MobileNo, 7, 3) + '-' + RIGHT(MobileNo, 4) 
--FROM	EMPLOYEE_MASTER 
--WHERE	ISRESIGNED=0 AND MobileNo LIKE '%+971%' AND LEN(MobileNo) = 13


--update	EMPLOYEE_MASTER
--SET		MobileNo = LEFT(MobileNo, 4) + '-' + SUBSTRING(MobileNo, 5, 2) + '-' + SUBSTRING(MobileNo, 7, 3) + '-' + RIGHT(MobileNo, 4) 
--WHERE	ISRESIGNED=0 AND MobileNo LIKE '%+971%' AND LEN(MobileNo) = 13

--update	EMPLOYEE_MASTER
--SET		MobileNo = replace(MobileNo, '-', '')
--WHERE	ISRESIGNED=0 AND MobileNo LIKE '%971%' 
-- AND	MobileNo LIKE '%-%'

--update EMPLOYEE_MASTER
--set MobileNo = replace(MobileNo, '05', '+9715')
--WHERE ISRESIGNED=0 --AND MobileNo LIKE '%971%'
-- AND MobileNo LIKE '05%'

--update EMPLOYEE_MASTER
--set MobileNo = replace(MobileNo, '00', '+')
--WHERE ISRESIGNED=0 AND MobileNo LIKE '%971%'
-- AND MobileNo LIKE '%00971%'

--update EMPLOYEE_MASTER
--set MobileNo = replace(MobileNo, ' ', '')
--WHERE ISRESIGNED=0 AND MobileNo LIKE '%971%' AND MobileNo LIKE '% %'



--update EMPLOYEE_MASTER
--set MobileNo = '+'+MobileNo
--WHERE ISRESIGNED=0 AND MobileNo LIKE '971%'