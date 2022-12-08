import React from 'react'
import { ListElement } from '../components/list/list.tsx'
import { getImageSrc } from '../imageSources.ts'

export const HolderTransactionsListElement = ({transaction, currentAccount, openTransactionDetail}) => {

    return <ListElement>
        <div className="col-1 align-middle">
            { transaction.unitImageId && <img style={{maxHeight: 70, maxWidth: 70}} src={getImageSrc(transaction.unitImageId)}></img> }
        </div>
        <div className="col-4 align-middle">
            <p className="mb-0">Ticker: {transaction.unitTicker}</p>
            <p className='mb-0'>Issuer: {transaction.unitIssuerName} ({transaction.unitIssuerEmail})</p>
        </div>
        <div className="col-1 align-middle">
            {(currentAccount.id == transaction.creditorAccountId ? transaction.debtorImageId : transaction.creditorImageId) && <img style={{maxHeight: 70, maxWidth: 70}} src={getImageSrc(currentAccount.id == transaction.creditorAccountId ? transaction.debtorImageId : transaction.creditorImageId)}></img> }
        </div>
        <div className="col-4 align-middle">
            <p className="mb-0">{transaction.timestamp.substring(0,19).replaceAll('T', ' ')}</p>
            <p className="mb-0">Counterparty: { currentAccount.id == transaction.creditorAccountId ? `${transaction.debtorName} (${transaction.debtorEmail})` : `${transaction.creditorName} (${transaction.creditorEmail})`}</p>
            <p className='mb-0'>{`Amount: `}
                <b style={{color: currentAccount.id == transaction.creditorAccountId ? "red" : "#4CBB17"}}>
                    {transaction.amount}
                </b>
            </p>
        </div>
        <div className=" col-1">
        </div>
        <div className=" col-1">
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15}} title={"Detail"} onClick={() => openTransactionDetail(transaction)}>
                Detail
            </button>
        </div>
    </ListElement>
}