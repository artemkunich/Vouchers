import React, {useState, useCallback } from 'react'
import { Header} from '../components/common/header.tsx'
import { HolderTransactionDetailContainer} from '../components/holderTransactionDetail/container.js'
import { HolderTransactionsList } from '../components/holderTransactions/list.js'

export const PagesHolderTransactions = () => {
    
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
            <HolderTransactionDetailContainer transaction={transactionToOpenDetail} handleCancel={handleCancel}/>
            :
            <></>
        }
    </>
}