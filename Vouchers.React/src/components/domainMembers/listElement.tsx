import * as React from 'react'
import { ListElement } from '../common/list/list'
import { getImageSrc } from '../../api/imageSources'
import "bootstrap-icons/font/bootstrap-icons.css";
import { DomainAccountDto } from '../../types/dtos';
import { UpdateDomainAccountCommand } from '../../types/commands';

interface DomainMembersListElementProps {
    currentAccount: DomainAccountDto, 
    domainAccount: DomainAccountDto, 
    putDomainAccount: (account: UpdateDomainAccountCommand) => Promise<void>, 
    openDomainAccountDetail: (account: DomainAccountDto) => void, 
}

export const DomainMembersListElement = ({currentAccount, domainAccount, putDomainAccount, openDomainAccountDetail}: DomainMembersListElementProps) => {

    return <ListElement>
        <div className="container">
            <a title={domainAccount.name}>
                <div className="row mb-2">
                    <div className="col-2 align-middle">
                        { domainAccount.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(domainAccount.imageId)}></img> }
                    </div>
                    <div className="col-8 align-middle">
                        <p className="fs-5">{domainAccount.email}</p>
                        <p className="fs-5">{domainAccount.name}</p>
                    </div>
                    <div className=" col-2">
                        <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Detail"} onClick={async () => openDomainAccountDetail(domainAccount)}>
                            Detail
                        </button>
                        {
                            (currentAccount.isAdmin || currentAccount.isOwner) &&
                            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={domainAccount.isConfirmed ? "Remove" : "Confirm"} onClick={async () => await putDomainAccount({domainAccountId: domainAccount.id, isConfirmed: !domainAccount.isConfirmed})}>
                                {domainAccount.isConfirmed ? "Remove" : "Confirm"}
                            </button>
                        }                        
                    </div>        
                </div>
            </a> 
        </div>
    </ListElement>
}