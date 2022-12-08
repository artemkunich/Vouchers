import React, {useState, useCallback } from 'react'
import { Header} from '../components/header/header.tsx'
import { HolderTransactionDetail} from '../holderTransactionDetail/holderTransactionDetail.js'
import { HolderTransactionsList } from '../holderTransactions/HolderTransactionsList.js'

export const HolderTransactionsContainer = () => {
    
    const [transactionToOpenDetail, setTransactionToOpenDetail] = useState();

    const handleCancel = useCallback(async () => {
        setTransactionToOpenDetail()
    }, [])

    return <>
        <Header>Holder transactions</Header>

        <div style={{display: (transactionToOpenDetail) ? "none" : "block"}}>
            <HolderTransactionsList openTransactionDetail={setTransactionToOpenDetail}/>
        </div>
        
        {
            transactionToOpenDetail ?
            <HolderTransactionDetail transaction={transactionToOpenDetail} handleCancel={handleCancel}/>
            :
            <></>
        }
    </>
}