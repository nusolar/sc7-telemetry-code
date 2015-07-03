DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `p_uiGetHistoryData`(FromDate DateTime, ToDate DateTime, MaxRows Integer)
BEGIN
        IF FromDate IS NOT NULL AND ToDate IS NOT NULL THEN
            SELECT * FROM history WHERE ts1 BETWEEN FromDate AND ToDate ORDER BY ts1 ASC;
        ELSE
            IF FromDate IS NOT NULL THEN
                SELECT * FROM history WHERE ts1 >= FromDate ORDER BY ts1 ASC;
            ELSE
                IF ToDate IS NOT NULL THEN
                    SELECT * FROM history WHERE ts1 <= ToDate ORDER BY ts1 ASC;
                ELSE
                    IF MaxRows IS NOT NULL THEN
                        SELECT * FROM history ORDER BY ts1 ASC LIMIT MaxRows;
                    ELSE
                        SELECT * FROM history ORDER BY ts1 ASC;
                    END IF;
                END IF;
            END IF;
        END IF;
        
END