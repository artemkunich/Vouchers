import * as React from 'react'

interface FetchingListItemsInfoProps {
    isFetching: boolean
}

export const FetchingListItemsInfo = ({isFetching}: FetchingListItemsInfoProps) => <>{isFetching && 'Fetching more list items...'}</>