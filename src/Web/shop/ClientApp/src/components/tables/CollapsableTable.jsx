import React from 'react';
import { Table } from 'antd';

const CollapsableTable = ({
  columns,
  expandedColumns,
  data
}) => {
  const expandedRowRender = record => {
    const data = record.collapseData;
    return <Table columns={expandedColumns} dataSource={data} pagination={false} />;
  };

  return (
    <Table
      className="components-table-demo-nested"
      columns={columns}
      expandedRowRender={expandedRowRender}
      dataSource={data}
    />
  );
};

export default CollapsableTable;
