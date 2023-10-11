# 随手记帐单转换工具

## 介绍
将常见银行帐单转换城随手记的导入格式！

![image-20230924153829466](docs/images/image-20230924153829466.png)

### 支持清单
- 微信帐单
- 支付宝
- 中国农业银行
- 中国工商银行
- 招商银行

## 部署
```shell
# 拉取镜像
docker pull risenzhang/feideeparser:latest

# 运行镜像
docker run --name feideeparser -d -p 52001:5000 risenzhang/feideeparser:latest
```
打开网页，假设部署在本地电脑中输入「http://127.0.0.1:52001」其中端口可以根据自己需求调整